﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Observers
{
    public class BuffButtonObserver : MonoBehaviour, IObserver<bool>
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private Unit unit;

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(bool value)
        {
            button.interactable = value;
        }

        private void Awake()
        {
            unit.isReadyToBuff.Subscribe(this);
        }
    }
}