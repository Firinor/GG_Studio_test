using UniRx;

namespace Buffs
{
    public class Buff
    {
        public string Name { get; private set; }
        public IntReactiveProperty Duration { get; private set; }
        public bool IsOver => Duration.Value <= 0;

        private Unit owner;
        public BuffCore BuffCore { get; private set; }

        public void Start(Unit unit, BuffCore BuffCore)
        {
            owner = unit;
            this.BuffCore = BuffCore;
            Name = BuffCore.name;
            Duration = new IntReactiveProperty(BuffCore.Duration);

            BuffCore.OnStart(owner);
        }
        public void Tick()
        {
            Duration.Value--;

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