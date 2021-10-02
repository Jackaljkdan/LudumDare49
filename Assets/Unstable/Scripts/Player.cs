using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class Player : MonoBehaviour
    {
        #region Inspector

        public float stepMovement = 0.28f;

        [Header("Runtime")]

        [SerializeField]
        private bool moving = false;

        [SerializeField]
        private bool isNextStepR = true;

        [ContextMenu("Lose balance")]
        private void InspectorLoseBalance()
        {
            if (!Application.isPlaying)
                return;

            OnBalanceLost();
        }

        #endregion

        private Tween stepTween;

        [Inject]
        private Balance balance = null;

        private void Start()
        {
            balance.onBalanceLost.AddListener(OnBalanceLost);
        }

        private void Update()
        {
            if (moving)
                return;

            if (UnityEngine.Input.GetKey(KeyCode.W))
                StepForward();
        }

        public void StepForward()
        {
            var animator = GetComponent<Animator>();

            string animName = isNextStepR ? "WalkR" : "WalkL";
            animator.Play(animName, 0, 0);

            stepTween = transform.DOMove(transform.position + Vector3.forward * stepMovement, duration: 1 / animator.speed)
                    .SetEase(Ease.Linear);

            stepTween.onComplete += OnStepCompleted;

            isNextStepR = !isNextStepR;

            moving = true;
        }

        private void OnStepCompleted()
        {
            moving = false;
        }

        
        private void OnBalanceLost()
        {
            if (stepTween != null && stepTween.active)
                stepTween.Kill();

            enabled = false;
            GetComponent<Animator>().enabled = false;

            foreach (var rb in GetComponentsInChildren<Rigidbody>())
                rb.constraints = RigidbodyConstraints.None;
        }
    }
    
    [Serializable]
    public class UnityEventPlayer : UnityEvent<Player> { }
}