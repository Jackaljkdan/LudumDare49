using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class Foot : MonoBehaviour
    {
        #region Inspector



        #endregion

        [Inject]
        private Balance balance = null;

        private void Start()
        {
            balance.onBalanceLost.AddListener(OnBalanceLost);
        }

        private void OnBalanceLost()
        {
            Destroy(GetComponent<CharacterJoint>());
        }
    }
    
    [Serializable]
    public class UnityEventFoot : UnityEvent<Foot> { }
}