using UnityEngine;

namespace Buffs
{
    [CreateAssetMenu(fileName = "DecoratorBuff", menuName = "Buffs/DecoratorBuff")]
    public class DecoratorBuff : BuffCore
    {
        public UnitAttributes attributes;

        public override void Decorate(AttackData attackData)
        {
            foreach (var attribute in attributes)
            {
                if (attackData.ContainsKey(attribute.Key))
                    attackData[attribute.Key] += attribute.Value;
                else
                    attackData.Add(attribute.Key, attribute.Value);
            }
        }
    }
}

