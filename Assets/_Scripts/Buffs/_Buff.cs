using System;
using UnityEngine;

namespace Buffs
{
    public class Buff
    {
        private int duration;
        public bool IsOver => duration <= 0;

        private Unit owner;
        public BuffCore BuffCore { get; private set; }

        public void Start(Unit unit, BuffCore BuffCore)
        {
            owner = unit;
            this.BuffCore = BuffCore;
            duration = BuffCore.duration;

            BuffCore.OnStart(owner);
        }
        public void Tick()
        {
            duration--;

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