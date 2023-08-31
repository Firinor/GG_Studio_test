using UnityEngine;

namespace Buffs
{
    [CreateAssetMenu(fileName = "DecoratorBuff", menuName = "Buffs/DecoratorBuff")]
    public class DecoratorBuff : BuffCore
    {
        public UnitAttribute[] attributes;

        public override void Decorate(AttackData attackData)
        {
            foreach (var i in attributes)
            {
                if (attackData.ContainsKey(i.Attribute))
                    attackData[i.Attribute] += i.Value;
                else
                    attackData.Add(i.Attribute, i.Value);
            }
        }
    }
}

