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

        public float rotationLerp = 0.1f;

        [Range(0f, 10)]
        [SerializeField]
        private float _speed = 1;

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

        private void OnValidate()
        {
            Speed = _speed;
        }

        #endregion

        private Tween stepTween;

        public float Speed
        {
            get => GetComponent<Animator>().speed;
            set => GetComponent<Animator>().speed = value;
        }

        private Checkpoint nextCheckpoint;

        [Inject]
        private Balance balance = null;
        
        [Inject]
        private void Inject([Inject(Id = "first")] Checkpoint firstCheckpoint)
        {
            nextCheckpoint = firstCheckpoint;
        }

        private void Start()
        {
            Speed = _speed;
            balance.onBalanceLost.AddListener(OnBalanceLost);
        }

        private void Update()
        {
            if (moving)
            {
                float speed = Speed;

                Vector3 movement;

                if (nextCheckpoint != null)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, nextCheckpoint.transform.rotation, Time.deltaTime * rotationLerp * speed);
                    movement = (nextCheckpoint.transform.position - transform.position).normalized;
                }
                else
                {
                    movement = transform.TransformDirection(Vector3.forward);
                }

                transform.position = transform.position + movement * stepMovement * Time.deltaTime * speed;

                if (nextCheckpoint != null && (transform.position - nextCheckpoint.transform.position).sqrMagnitude < 0.0001f)
                    nextCheckpoint = nextCheckpoint.next;

                return;
            }

            BeginMoving();
        }

        public void BeginMoving()
        {
            var animator = GetComponent<Animator>();

            string animName = isNextStepR ? "WalkR" : "WalkL";
            animator.Play(animName, 0, 0);

            isNextStepR = !isNextStepR;

            moving = true;
        }

        public void OnStepCompleted()
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