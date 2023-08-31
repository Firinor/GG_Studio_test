using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Observer
{
    public class RoundTextObserver : MonoBehaviour, IObserver<int>
    {
        [Inject]
        private GameManager gameManager;
        [SerializeField]
        private TextMeshProUGUI text;

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(int value)
        {
            text.text = $"Round {value}";
        }

        private void OnEnable()
        {
            gameManager.RoundCount.Subscribe(this);
        }
    }
}
