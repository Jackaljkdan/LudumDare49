using JK.Sounds;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class Cannon : MonoBehaviour
    {
        #region Inspector

        public float maxPlayerDistance = 6;

        public float turnLerp = 0.2f;

        public float dotToShoot = 0.95f;

        public float secondsBetweenShots = 4;

        public float force = 100;

        public float playerSpeedMultiplier = 0;

        public float playerDistanceMultiplier = 1;

        public float dotToDeactivate = 0.1f;

        public float deactivationDelaySeconds = 2f;

        public Rigidbody projectilePrefab;


        [Header("Internals")]

        [SerializeField]
        private Transform projectileAnchor = null;

        [SerializeField]
        private ParticleSystem particles = null;

        #endregion

        private DateTime lastShot;

        [Inject]
        private Player player = null;

        private void Start()
        {
            lastShot = DateTime.Now - TimeSpan.FromSeconds(secondsBetweenShots + 1);
            particles.Stop();
        }

        private void Update()
        {
            float distance = (player.transform.position - transform.position).magnitude;

            if (distance > maxPlayerDistance)
                return;

            Vector3 offset = player.transform.TransformVector(Vector3.up) * distance * playerDistanceMultiplier;

            Vector3 targetPosition = player.transform.position + offset + player.transform.forward * player.Speed * playerSpeedMultiplier;

            Vector3 toTarget = targetPosition - transform.position;
            Vector3 directionToTarget = toTarget.normalized;

            Vector3 updatedForward = Vector3.Lerp(transform.forward, directionToTarget, turnLerp);
            transform.forward = updatedForward;

            if ((DateTime.Now - lastShot).TotalSeconds < secondsBetweenShots)
                return;

            float dot = Vector3.Dot(updatedForward, directionToTarget);

            if (dot >= dotToShoot)
            {
                var projectile = Instantiate(projectilePrefab, projectileAnchor.position, projectileAnchor.rotation, transform.parent);
                projectile.AddForce(projectileAnchor.forward * force, ForceMode.Impulse);
                lastShot = DateTime.Now;
                particles.Stop();
                particles.Play();

                if (projectileAnchor.TryGetComponent(out RandomClipsPlayer clips))
                    clips.PlayRandom();
            }

            dot = Vector3.Dot(updatedForward, player.transform.forward);

            if (dot >= dotToDeactivate)
            {
                enabled = false;
                Invoke(nameof(Deactivate), deactivationDelaySeconds);
            }
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
    
    [Serializable]
    public class UnityEventCannon : UnityEvent<Cannon> { }
}