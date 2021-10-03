using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Player))]
    public class Accelerator : MonoBehaviour
    {
        #region Inspector

        public float minInertia = 0.02f;
        public float maxInertia = 0.06f;

        public float overSeconds = 10;

        [SerializeField]
        private float startTime = 0;

        #endregion

        [Inject]
        private Balance balance = null;

        private void Start()
        {
            balance.onRestored.AddListener(OnBalanceRestored);
            balance.onBalanceLost.AddListener(OnBalanceLost);
            startTime = Time.time;
        }

        private void Update()
        {
            float t = Mathf.Min(1, (Time.time - startTime) / overSeconds);
            balance.maxInertia = Mathf.Lerp(minInertia, maxInertia, t);
        }

        private void OnBalanceRestored()
        {
            startTime = Time.time;
            balance.maxInertia = minInertia;
            enabled = true;
        }

        private void OnBalanceLost()
        {
            enabled = false;
        }
    }
    
    [Serializable]
    public class UnityEventAccelerator : UnityEvent<Accelerator> { }
}