using System.Collections.Generic;
using UnityEngine;

namespace Buffs
{
    [CreateAssetMenu(fileName = "AttributeBuff", menuName = "Buffs/AttributeBuff")]
    public class AttributeBuff : BuffCore
    {
        public UnitAttribute[] attributes;

        public override void OnStart(Unit unit)
        {
            foreach (var attribute in attributes)
            {
                unit.AddToAttribute(new KeyValuePair<Attribute, float>(attribute.Attribute, attribute.Value));
            }
        }

        public override void OnEnd(Unit unit)
        {
            foreach (var attribute in attributes)
            {
                unit.RemoveFromAttribute(new KeyValuePair<Attribute, float>(attribute.Attribute, attribute.Value));
            }
        }
    }
}

