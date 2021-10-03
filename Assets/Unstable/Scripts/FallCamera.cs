using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class FallCamera : MonoBehaviour
    {
        #region Inspector

        public float lerp = 0.2f;

        #endregion

        [Inject]
        private Balance balance = null;

        [Inject]
        private Player player = null;

        private Quaternion initialLocalRotation;

        private void Start()
        {
            initialLocalRotation = transform.localRotation;

            balance.onBalanceLost.AddListener(OnBalanceLost);
            balance.onRestored.AddListener(OnBalanceRestored);
            enabled = false;
        }

        private void Update()
        {
            transform.forward = Vector3.Lerp(transform.forward, player.body.torso.transform.position - transform.position, lerp);
        }

        private void OnBalanceLost()
        {
            enabled = true;
        }

        private void OnBalanceRestored()
        {
            enabled = false;
            transform.localRotation = initialLocalRotation;
        }
    }
    
    [Serializable]
    public class UnityEventFallCamera : UnityEvent<FallCamera> { }
}