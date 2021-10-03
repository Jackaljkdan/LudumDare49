using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class Checkpoint : MonoBehaviour
    {
        #region Inspector

        public Checkpoint next;

        public bool canSave = true;

        #endregion

        private void OnDrawGizmos()
        {
            Color color = next == null ? Color.red : Color.green;

            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, 0.2f);

            if (next != null)
                Gizmos.DrawLine(transform.position, next.transform.position);
            else
                Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        }
    }
    
    [Serializable]
    public class UnityEventCheckpoint : UnityEvent<Checkpoint> { }
}