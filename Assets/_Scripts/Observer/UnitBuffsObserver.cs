using Buffs;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static TMPro.TMP_Dropdown;

namespace Observers
{
    public class UnitBuffsObserver : MonoBehaviour, IObserver<int>
    {
        [SerializeField]
        private Unit unit;
        [SerializeField]
        private TMP_Dropdown dropdown;

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(int value)
        {
            dropdown.ClearOptions();
            var newOptions = new List<OptionData>();
            //We leave the zero index under the heading
            newOptions.Add(new OptionData("Buffs"));

            foreach (Buff buff in unit.Buffs)
            {
                newOptions.Add(new OptionData($"{buff.Name} ({buff.Duration})"));
            }

            dropdown.AddOptions(newOptions);
        }

        private void Awake()
        {
            unit.Buffs.ObserveCountChanged().Subscribe(this);
        }
    }
}
