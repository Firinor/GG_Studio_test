using System;
using UniRx;

namespace Utility.UniRx
{
    public class LimitedFloatReactiveProperty : FloatReactiveProperty
    {
        public bool MinLimit;
        public float MinValue;
        public bool MaxLimit;
        public float MaxValue;
        public new float Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if (!MinLimit)
                    MinValue = float.MinValue;
                if (!MaxLimit)
                    MaxValue = float.MaxValue;

                float _value = Math.Clamp(value, MinValue, MaxValue);
                base.Value = _value;
            }
        }

        public LimitedFloatReactiveProperty() : base()
        {
            MinLimit = false;
            MaxLimit = false;
        }
        public LimitedFloatReactiveProperty(float value) : base(value)
        {
            MinLimit = false;
            MaxLimit = false;
        }
        public LimitedFloatReactiveProperty(float value, float min, float max) : base(value)
        {
            MinValue = min;
            MaxValue = max;
        }
    }
}
