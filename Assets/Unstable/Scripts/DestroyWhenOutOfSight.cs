using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unstable
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public class DestroyWhenOutOfSight : MonoBehaviour
    {
        #region Inspector

        public float delaySeconds = 1;

        #endregion

        private void OnBecameInvisible()
        {
            Invoke(nameof(DestroyIfInvisible), delaySeconds);
        }

        private void DestroyIfInvisible()
        {
            if (!GetComponent<Renderer>().isVisible)
                Destroy(gameObject);
        }
    }
    
    [Serializable]
    public class UnityEventDestroyWhenOutOfSight : UnityEvent<DestroyWhenOutOfSight> { }
}