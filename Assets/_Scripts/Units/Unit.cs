using Buffs;
using System;
using System.Collections.Generic;
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
    private UnitAttributes currentStats;

    public bool isReady { get; private set; }

    private float HealthPoint
    {
        get { return currentStats[Attribute.Health]; }
        set
        {
            currentStats[Attribute.Health] = Math.Min(value, basisStats.MaxHealthPoint);
            if (value <= 0)
                gameManager.GameOver();
        }
    }
    public bool IsDead => currentStats[Attribute.Health] <= 0;

    private List<Buff> buffs;

    private void Awake()
    {
        currentStats = new UnitAttributes();
        currentStats[Attribute.Attack] = basisStats.Attack;
        currentStats[Attribute.Defence] = basisStats.DefenceRate;
        currentStats[Attribute.Health] = basisStats.HealthPoint;
        currentStats[Attribute.Vampiric] = basisStats.VampiricRate;
    }

    public void Attack()
    {
        //if(MissCheck()) return;

        AttackData damage = GenerateAttackData();

        //BoostWithEquipment(damage);
        //BoostWithCrit(damage);
        BoostWithBuffs(ref damage);

        float totalDamageDone = target.TakeHit(damage);

        if (currentStats[Attribute.Vampiric] <= 0)
            return;

        float vampiricHeal = totalDamageDone * Ratios.PercentagesInRates(currentStats[Attribute.Vampiric]);

        HealthPoint += vampiricHeal;
    }

    private AttackData GenerateAttackData()
    {
        return new AttackData() { 
            {Attribute.Attack, currentStats[Attribute.Attack]} 
        };
    }

    private void BoostWithBuffs(ref AttackData damage)
    {
        foreach(Buff buff in buffs)
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
            currentStats[attribute] += value;
        else
            currentStats.Add(attribute, value);
    }


    internal void DeactivateAll()
    {
        isReady = false;
    }

    private float TakeHit(AttackData attackData)
    {
        float totalDamageDone = 0;

        foreach (var attack in attackData)
        {
            if (attack.Key == Attribute.Attack)
                totalDamageDone = TakeDamage(attack.Value * attackData.multiplicator);
            else
                currentStats[attack.Key] -= attack.Value * attackData.multiplicator;
        }

        return totalDamageDone;
    }
    private float TakeDamage(float damage)
    {
        damage = Ratios.ReduceByPercentage(damage, currentStats[Attribute.Defence]);

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
        BuffCore[] result = new BuffCore[buffs.Count];

        for(int i = 0; i < buffs.Count; i++)
        {
            result[i] = buffs[i].BuffCore;
        }

        return result;
    }

    public void AddBuff(BuffCore buffBehaviour)
    {
        Buff buff = new Buff();

        buffs.Add(buff);
        buff.Start(this, buffBehaviour);
    }
}
