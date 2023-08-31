using UnityEngine;

namespace Buffs
{
    public abstract class BuffCore : ScriptableObject
    {
        public int Duration;
        public virtual void OnStart(Unit unit) { }
        public virtual void Tick(Unit unit) { }
        public virtual void OnEnd(Unit unit) { }
        public virtual void Decorate(AttackData attackData) { }
    }
}
