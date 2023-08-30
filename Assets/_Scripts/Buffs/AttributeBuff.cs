using UnityEngine;

namespace Buffs
{
    [CreateAssetMenu(fileName = "AttributeBuff", menuName = "Buffs/AttributeBuff")]
    public class AttributeBuff : BuffCore
    {
        public UnitAttributes attributes;

        public override void OnStart(Unit unit)
        {
            foreach (var attribute in attributes)
            {
                unit.AddToAttribute(attribute);
            }
        }

        public override void OnEnd(Unit unit)
        {
            foreach (var attribute in attributes)
            {
                unit.RemoveFromAttribute(attribute);
            }
        }
    }
}

