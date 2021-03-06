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

        public PlayerBody body;

        [Range(0f, 10)]
        [SerializeField]
        private float _speed = 1;

        [SerializeField]
        private Checkpoint dbgStart = null;

        [Header("Runtime")]

        [SerializeField]
        private bool moving = false;

        [SerializeField]
        private bool isNextStepR = true;

        public Checkpoint prevCheckpoint;
        public Checkpoint nextCheckpoint;

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
            get => body.GetComponent<Animator>().speed;
            set => body.GetComponent<Animator>().speed = value;
        }

        private Vector3 initialPosition;
        private Quaternion initialRotation;

        [Inject]
        private Balance balance = null;

        [Inject(Id = "first")]
        Checkpoint firstCheckpoint = null;

        private void Start()
        {
            Speed = _speed;
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            balance.onBalanceLost.AddListener(OnBalanceLost);
            body.onStepComplete.AddListener(OnStepCompleted);

            nextCheckpoint = firstCheckpoint;

            if (dbgStart != null && Application.isEditor)
            {
                transform.position = dbgStart.transform.position;
                nextCheckpoint = dbgStart;
                prevCheckpoint = dbgStart;
            }
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
                {
                    if (nextCheckpoint.canSave)
                        prevCheckpoint = nextCheckpoint;

                    nextCheckpoint = nextCheckpoint.next;
                }

                return;
            }

            BeginMoving();
        }

        public void BeginMoving()
        {
            var animator = body.GetComponent<Animator>();

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
            body.GetComponent<Animator>().enabled = false;
            body.fallingSounds.PlayRandom();

            foreach (var rb in GetComponentsInChildren<Rigidbody>())
                rb.constraints = RigidbodyConstraints.None;

            moving = false;
        }

        public void Respawn(PlayerBody newBody)
        {
            body = newBody;
            enabled = true;

            body.onStepComplete.AddListener(OnStepCompleted);

            var animator = body.GetComponent<Animator>();
            animator.enabled = true;
            animator.Play("Idle");

            Speed = _speed;

            PlaceAtLastCheckpoint();
        }

        public void PlaceAtLastCheckpoint()
        {
            if (prevCheckpoint == null)
            {
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                nextCheckpoint = firstCheckpoint;
            }
            else
            {
                transform.position = prevCheckpoint.transform.position;
                transform.rotation = prevCheckpoint.transform.rotation;
                nextCheckpoint = prevCheckpoint.next;
            }
        }
    }
    
    [Serializable]
    public class UnityEventPlayer : UnityEvent<Player> { }
}