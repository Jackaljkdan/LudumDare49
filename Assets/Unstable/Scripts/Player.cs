using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        #endregion

        private Tween stepTween;

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
    }
    
    [Serializable]
    public class UnityEventPlayer : UnityEvent<Player> { }
}