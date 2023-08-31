using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Observers
{
    public class SliderButtonObserver : MonoBehaviour, IObserver<float>
    {
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private Unit unit;
        [SerializeField]
        private Attribute attribute;
        [SerializeField]
        private TextMeshProUGUI text;

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(float value)
        {
            slider.value = value;
            text.text = $"{(int)value} / 100";
        }

        private void OnEnable()
        {
            unit[attribute].Subscribe(this);
        }
    }
}