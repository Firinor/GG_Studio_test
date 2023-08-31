using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Observers
{
    public class UnitLostHealthObserver : MonoBehaviour, IObserver<float>
    {
        [SerializeField]
        private Unit unit;
        [SerializeField]
        private Attribute attribute;
        [SerializeField]
        private Image image;
        [SerializeField]
        private Color toColor;

        private float lastValue;

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(float value)
        {
            if (value < lastValue)
            {
                Sequence sequence = DOTween.Sequence();

                var toEndColor = DOTween.To(() => Color.white, x => image.color = x, toColor, 0.5f);
                var toWhiteColor = DOTween.To(() => toColor, x => image.color = x, Color.white, 0.5f);

                sequence.Join(toEndColor);
                sequence.Join(toWhiteColor);
                sequence.Play();
            }

            lastValue = value;
        }

        private void OnEnable()
        {
            unit[attribute].Subscribe(this);
            lastValue = unit[attribute].Value;
        }
    }
}