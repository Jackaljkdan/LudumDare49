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

        #endregion

        [Inject]
        private Balance balance = null;

        private void LateUpdate()
        {
            var rot = transform.localEulerAngles;
            rot.z = balanceRotationMultiplier * balance.balance * balance.maxRotation;
            transform.localEulerAngles = rot;
        }
    }
}