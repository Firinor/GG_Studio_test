using Buffs;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Utility;
using Utility.UniRx;
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
        {Attribute.Attack, new LimitedFloatReactiveProperty() },
        {Attribute.Defence, new LimitedFloatReactiveProperty() },
        {Attribute.Health, new LimitedFloatReactiveProperty() },
        {Attribute.Vampiric, new LimitedFloatReactiveProperty() } 
    };

    public ReactiveCollection<Buff> Buffs;

    public BoolReactiveProperty isReady;
    public BoolReactiveProperty isReadyToBuff;
    public bool IsDead => currentStats[Attribute.Health].Value <= 0;

    private float HealthPoint
    {
        get { return currentStats[Attribute.Health].Value; }
        set
        {
            currentStats[Attribute.Health].Value = value;
            if (value <= 0)
                gameManager.GameOver();
        }
    }
    public LimitedFloatReactiveProperty this[Attribute key] => currentStats[key];


    private void Awake()
    {
        InitStats();
    }

    private void InitStats()
    {
        InitAttack();
        InitHealth();
        InitDefence();
        InitVampiric();
    }

    private void InitAttack()
    {
        var stat = currentStats[Attribute.Attack];
        stat.MinLimit = true;
        stat.MinValue = 1;
        stat.Value = basisStats.Attack;
    }
    private void InitHealth()
    {
        var stat = currentStats[Attribute.Health];
        stat.MaxLimit = true;
        stat.MaxValue = basisStats.MaxHealthPoint;
        stat.Value = basisStats.HealthPoint;
    }
    private void InitDefence()
    {
        var stat = currentStats[Attribute.Defence];
        stat.MinLimit = true;
        stat.MinValue = 0;
        stat.MaxLimit = true;
        stat.MaxValue = basisStats.MaxDefenceRate;
        stat.Value = basisStats.DefenceRate;

    }
    private void InitVampiric()
    {
        var stat  = currentStats[Attribute.Vampiric];
        stat.MinLimit = true;
        stat.MinValue = 0;
        stat.MaxLimit = true;
        stat.MaxValue = basisStats.MaxVampiricRate;
        stat.Value = basisStats.VampiricRate;
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

        isReady.Value = true;
        isReadyToBuff.Value = Buffs.Count < basisStats.BuffLimit;
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
        currentStats[attribute].Value += value;
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
