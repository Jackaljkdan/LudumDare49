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

        public Transform target;

        public float lerp = 0.2f;

        #endregion

        [Inject]
        private void Inject(Balance balance)
        {
            balance.onBalanceLost.AddListener(OnBalanceLost);
        }

        private void OnBalanceLost()
        {
            StartCoroutine(LookAtTargetCoroutine());
        }

        private IEnumerator LookAtTargetCoroutine()
        {
            while (true)
            {
                transform.forward = Vector3.Lerp(transform.forward, target.position - transform.position, lerp);
                yield return null;
            }
        }
    }
    
    [Serializable]
    public class UnityEventFallCamera : UnityEvent<FallCamera> { }
}