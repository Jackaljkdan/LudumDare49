using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Player))]
    public class InterferencePlayerRandomizer : MonoBehaviour
    {
        #region Inspector



        #endregion

        [Inject]
        private Interference interference = null;

        private void Start()
        {
            interference.onEnd.AddListener(OnInterferenceEnd);
        }

        private void OnInterferenceEnd()
        {
            var player = GetComponent<Player>();

            if (player.enabled)
                player.body.RandomizeColors();
        }
    }
    
    [Serializable]
    public class UnityEventInterferencePlayerRandomizer : UnityEvent<InterferencePlayerRandomizer> { }
}