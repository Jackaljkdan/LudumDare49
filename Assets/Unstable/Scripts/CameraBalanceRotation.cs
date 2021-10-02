using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class CameraBalanceRotation : MonoBehaviour
    {
        #region Inspector

        public float balanceRotationMultiplier = 1;

        public float lerp = 0.2f;

        public bool upsideDown = false;

        #endregion

        [Inject]
        private Balance balance = null;

        private void LateUpdate()
        {
            float target = balanceRotationMultiplier * balance.balance * balance.maxRotation;

            if (upsideDown)
                target = target + 180;

            var rot = transform.localEulerAngles;
            rot.z = Mathf.LerpAngle(rot.z, target, lerp);
            transform.localEulerAngles = rot;
        }
    }
}