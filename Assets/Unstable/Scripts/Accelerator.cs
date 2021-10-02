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

        public float maxSpeed = 6;
        public float minSpeed = 0.6f;

        public float inertiaLerp = 0.05f;
        public float inputLerp = 0.1f;

        public float maxInertia = 0.2f;

        public float balanceMultiplier = 0.99f;

        #endregion

        private float inertia;

        private float originalBalanceInertiaLerp;

        [Inject]
        private Balance balance = null;

        private void Start()
        {
            originalBalanceInertiaLerp = balance.inertiaLerp;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKey(KeyCode.W))
                inertia = Mathf.Lerp(inertia, 1, inputLerp);

            if (UnityEngine.Input.GetKey(KeyCode.S))
                inertia = Mathf.Lerp(inertia, -1, inputLerp);

            var player = GetComponent<Player>();
            float speed = player.Speed;

            float inertiaTarget = 1;
            inertia = Mathf.Lerp(inertia, inertiaTarget, inertiaLerp);

            inertia = Mathf.Clamp(inertia, -maxInertia, maxInertia);

            float updatedSpeed = Mathf.Clamp(speed + inertia, minSpeed, maxSpeed);
            player.Speed = updatedSpeed;

            balance.inertiaLerp = Mathf.Lerp(
                originalBalanceInertiaLerp,
                balance.inputLerp * balanceMultiplier,
                Math.Max(0, updatedSpeed - 1) / (maxSpeed - 1)
            );
        }
    }
    
    [Serializable]
    public class UnityEventAccelerator : UnityEvent<Accelerator> { }
}