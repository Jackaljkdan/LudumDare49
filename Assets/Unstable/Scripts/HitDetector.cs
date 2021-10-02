using JK.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unstable
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class HitDetector : MonoBehaviour
    {
        #region Inspector

        public UnityEventCollision onHit = new UnityEventCollision();

        #endregion

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Projectile"))
                onHit.Invoke(GetComponent<Collider>(), collision);
        }
    }
    
    [Serializable]
    public class UnityEventHitDetector : UnityEvent<HitDetector> { }
}