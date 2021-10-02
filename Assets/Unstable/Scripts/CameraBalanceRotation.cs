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

        #endregion

        [Inject]
        private Balance balance = null;

        private void LateUpdate()
        {
            var rot = transform.localEulerAngles;
            rot.z = Mathf.LerpAngle(rot.z, balanceRotationMultiplier * balance.balance * balance.maxRotation, lerp);
            transform.localEulerAngles = rot;
        }
    }
}