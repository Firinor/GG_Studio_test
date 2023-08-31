using System;
using UnityEngine;
using Zenject;

namespace Observers
{
    public class GameOverObserver : MonoBehaviour, IObserver<bool>
    {
        [Inject]
        private GameManager gameManager;
        [Inject]
        private GameOverBoard gameOverBoard;

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(bool value)
        {
            gameOverBoard.gameObject.SetActive(value);
        }

        private void Awake()
        {
            gameManager.IsGameOver.Subscribe(this);
        }
    }
}
