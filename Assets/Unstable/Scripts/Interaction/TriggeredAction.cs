using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unstable.Interaction
{
    public abstract class TriggeredAction : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private bool destroyAfterTriggering = true;

        [SerializeField]
        private bool destroyEntireGameobject = false;

        [SerializeField]
        private bool destroyColliderOnly = false;

        private void Reset()
        {
            if (!TryGetComponent(out Collider _))
                gameObject.AddComponent<BoxCollider>();

            GetComponent<Collider>().isTrigger = true;
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            PerformTriggeredAction();

            if (destroyAfterTriggering)
            {
                if (destroyEntireGameobject)
                    Destroy(gameObject);
                else if (destroyColliderOnly)
                    Destroy(GetComponent<Collider>());
                else
                    Destroy(this);
            }
        }

        protected abstract void PerformTriggeredAction();
    }

    [Serializable]
    public class UnityEventTriggeredAction : UnityEvent<TriggeredAction> { }
}