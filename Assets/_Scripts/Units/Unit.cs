using Buffs;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Utility;
using Zenject;

public class Unit : MonoBehaviour
{
    [Inject]
    private GameManager gameManager;
    [SerializeField]
    private Unit target;

    [SerializeField]
    private UnitBasisStats basisStats;
    private UnitAttributes currentStats = new UnitAttributes()
    {
        {Attribute.Attack, new FloatReactiveProperty() },
        {Attribute.Defence, new FloatReactiveProperty() },
        {Attribute.Health, new FloatReactiveProperty() },
        {Attribute.Vampiric, new FloatReactiveProperty() } 
    };

    public ReactiveCollection<Buff> Buffs;
    //private List<Buff> buffs = new List<Buff>();

    public BoolReactiveProperty isReady;
    public BoolReactiveProperty isReadyToBuff;
    public bool IsDead => currentStats[Attribute.Health].Value <= 0;

    private float HealthPoint
    {
        get { return currentStats[Attribute.Health].Value; }
        set
        {
            currentStats[Attribute.Health].Value = Math.Min(value, basisStats.MaxHealthPoint);
            if (value <= 0)
                gameManager.GameOver();
        }
    }
    public FloatReactiveProperty this[Attribute key] => currentStats[key];


    private void Awake()
    {
        currentStats[Attribute.Attack].Value = basisStats.Attack;
        currentStats[Attribute.Defence].Value = basisStats.DefenceRate;
        currentStats[Attribute.Health].Value = basisStats.HealthPoint;
        currentStats[Attribute.Vampiric].Value = basisStats.VampiricRate;
    }

    public void OnStartTurn()
    {
        for (int i = 0; ;)
        {
            if (i >= Buffs.Count)
                break;

            Buffs[i].Tick();
            if (Buffs[i].IsOver)
            {
                Buffs.Remove(Buffs[i]);
                continue;
            }

            i++;
        }
        Buffs.ObserveCountChanged();
        
        isReady.Value = true;
        isReadyToBuff.Value = true;
    }

    private void OnEndTurn()
    {
        isReady.Value = false;
        isReadyToBuff.Value = false;
    }

    public void Attack()
    {
        //if(MissCheck()) return;

        AttackData damage = GenerateAttackData();

        //BoostWithEquipment(damage);
        //BoostWithCrit(damage);
        BoostWithBuffs(ref damage);

        float totalDamageDone = target.TakeHit(damage);

        if (currentStats[Attribute.Vampiric].Value > 0)
        {
            float vampiricHeal = totalDamageDone * Ratios.PercentagesInRates(currentStats[Attribute.Vampiric].Value);
            HealthPoint += vampiricHeal;
        }

        OnEndTurn();
        gameManager.NextTurn();
        
    }

    private AttackData GenerateAttackData()
    {
        return new AttackData() { 
            {Attribute.Attack, currentStats[Attribute.Attack].Value} 
        };
    }

    private void BoostWithBuffs(ref AttackData damage)
    {
        foreach(Buff buff in Buffs)
        {
            buff.Decorate(damage);
        }
    }

    public void AddToAttribute(KeyValuePair<Attribute, float> attribute)
    {
        AddToAttribute(attribute.Key, attribute.Value);
    }
    public void RemoveFromAttribute(KeyValuePair<Attribute, float> attribute)
    {
        AddToAttribute(attribute.Key, -attribute.Value);
    }

    private void AddToAttribute(Attribute attribute, float value)
    {
        if (currentStats.ContainsKey(attribute))
            currentStats[attribute].Value += value;
        else
            currentStats.Add(attribute, new FloatReactiveProperty(value));
    }


    public void DeactivateAll()
    {
        OnEndTurn();
    }

    private float TakeHit(AttackData attackData)
    {
        float totalDamageDone = 0;

        foreach (var attack in attackData)
        {
            if (attack.Key == Attribute.Attack)
                totalDamageDone = TakeDamage(attack.Value * attackData.Multiplicator);
            else
                currentStats[attack.Key].Value += attack.Value * attackData.Multiplicator;
        }

        return totalDamageDone;
    }
    private float TakeDamage(float damage)
    {
        damage = Ratios.ReduceByPercentage(damage, currentStats[Attribute.Defence].Value);

        if (damage <= 0)
            return 0;

        float lostHealthPoint = Math.Min(damage, HealthPoint);

        HealthPoint -= damage;

        return lostHealthPoint;
    }

    public void CastRandomBuff()
    {
        BuffCore newBuff = gameManager.GetRandomBuffCore(filter: GetCurrentBuffCores());

        AddBuff(newBuff);
    }

    private BuffCore[] GetCurrentBuffCores()
    {
        BuffCore[] result = new BuffCore[Buffs.Count];

        for(int i = 0; i < Buffs.Count; i++)
        {
            result[i] = Buffs[i].BuffCore;
        }

        return result;
    }

    public void AddBuff(BuffCore buffBehaviour)
    {
        Buff buff = new Buff();
        buff.Start(this, buffBehaviour);

        Buffs.Add(buff);

        isReadyToBuff.Value = false;
    }
}
