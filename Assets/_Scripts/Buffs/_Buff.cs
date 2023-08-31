using System;
using UnityEngine;

namespace Buffs
{
    public class Buff
    {
        public string Name { get; private set; }
        public int Duration { get; private set; }
        public bool IsOver => Duration <= 0;

        private Unit owner;
        public BuffCore BuffCore { get; private set; }

        public void Start(Unit unit, BuffCore BuffCore)
        {
            owner = unit;
            this.BuffCore = BuffCore;
            Name = BuffCore.name;
            Duration = BuffCore.duration;

            BuffCore.OnStart(owner);
        }
        public void Tick()
        {
            Duration--;

            BuffCore.Tick(owner);

            if (IsOver)
                BuffCore.OnEnd(owner);
        }
        public void Stop() 
        {
            BuffCore.OnEnd(owner);
        }

        public void Decorate(AttackData damage)
        {
            BuffCore.Decorate(damage);
        }
    }
}