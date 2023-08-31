using UnityEngine;

namespace Buffs
{
    [CreateAssetMenu(fileName = "MultiplicatorBuff", menuName = "Buffs/MultiplicatorBuff")]
    public class MultiplicatorBuff : BuffCore
    {
        public int AllAttributesMultiplication = 1;

        public override void Decorate(AttackData attackData)
        {
            attackData.Multiplicator *= AllAttributesMultiplication;
        }
    }
}
