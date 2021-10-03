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

        public float angleOffset = 0;

        public float angleRangeIncrement = 10;

        public float counterRotationProbabilityIncrement = 0.05f;

        public float counterRotationMaxProbability = 0.33f;

        public int deathsBeforeChange = 0;

        [Header("Runtime")]

        [SerializeField]
        private int deathsCount = 0;

        [SerializeField]
        private float angleRange = 0;

        [SerializeField]
        private float counterRotationProbability = 0;

        #endregion

        [Inject]
        private Balance balance = null;

        [Inject]
        private Interference interference = null;

        private void Start()
        {
            balance.onBalanceLost.AddListener(OnBalanceLost);
            interference.onEnd.AddListener(OnInterferenceEnd);
        }

        private void LateUpdate()
        {
            float target = balanceRotationMultiplier * balance.balance * balance.maxRotation + angleOffset;

            var rot = transform.localEulerAngles;
            rot.z = Mathf.LerpAngle(rot.z, target, lerp);
            transform.localEulerAngles = rot;
        }

        private void OnBalanceLost()
        {
            deathsCount++;
        }

        private void OnInterferenceEnd()
        {
            if (deathsCount <= deathsBeforeChange)
                return;

            angleRange = Mathf.Min(angleRange + angleRangeIncrement, 180);
            angleOffset = UnityEngine.Random.Range(0, angleRange);

            counterRotationProbability = Mathf.Min(
                counterRotationProbability + counterRotationProbabilityIncrement,
                counterRotationMaxProbability
            );

            if (UnityEngine.Random.Range(0f, 1f) <= counterRotationProbability)
                balanceRotationMultiplier = -Mathf.Abs(balanceRotationMultiplier);
            else
                balanceRotationMultiplier = Mathf.Abs(balanceRotationMultiplier);
        }
    }
}