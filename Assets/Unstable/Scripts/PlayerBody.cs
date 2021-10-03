using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class PlayerBody : MonoBehaviour
    {
        #region Inspector

        public Transform upperBody;
        public Transform torso;

        public UnityEvent onStepComplete = new UnityEvent();

        #endregion

        public void OnStepCompleted()
        {
            onStepComplete.Invoke();
        }
    }

    [Serializable]
    public class UnityEventPlayerBody : UnityEvent<PlayerBody> { }
}