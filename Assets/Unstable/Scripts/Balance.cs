using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class Balance : MonoBehaviour
    {
        #region Inspector

        [Range(-1, 1)]
        public float balance;

        public float randomAmount = 0.01f;

        public float maxRotation = 45;

        public float inputLerp = 0.2f;
        public float inertiaLerp = 0.3f;

        public float maxInertia = 0.1f;

        public float inertia;

        public UnityEvent onBalanceLost = new UnityEvent();

        #endregion

        private void Update()
        {
            if (UnityEngine.Input.GetKey(KeyCode.A))
                inertia = Mathf.Lerp(inertia, 1, inputLerp);

            if (UnityEngine.Input.GetKey(KeyCode.D))
                inertia = Mathf.Lerp(inertia, -1, inputLerp);

            float sign = Mathf.Sign(balance);
            float inertiaTarget = sign;
            inertia = Mathf.Lerp(inertia, inertiaTarget, inertiaLerp);

            inertia = Mathf.Clamp(inertia, -maxInertia, maxInertia);

            balance += inertia;

            var rot = transform.localEulerAngles;
            rot.z = balance * maxRotation;
            transform.localEulerAngles = rot;

            //balance = Mathf.Clamp(balance, -1, 1);

            if (balance >= 1 || balance <= -1)
            {
                enabled = false;
                onBalanceLost.Invoke();
            }
        }
    }
    
    [Serializable]
    public class UnityEventBalance : UnityEvent<Balance> { }
}