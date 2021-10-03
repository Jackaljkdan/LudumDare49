using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class SunlightRandomizer : MonoBehaviour
    {
        #region Inspector

        public List<Vector3> angles = new List<Vector3>();

        [ContextMenu("Add current")]
        private void InspectorAddCurrent()
        {
            angles.Add(transform.eulerAngles);
        }

        #endregion

        [Inject]
        private Interference interference = null;

        private void Start()
        {
            interference.onEnd.AddListener(OnInterferenceEnd);
        }

        private void OnInterferenceEnd()
        {
            int randomIndex = UnityEngine.Random.Range(0, angles.Count);
            transform.eulerAngles = angles[randomIndex];
        }
    }
    
    [Serializable]
    public class UnityEventSunlightRandomizer : UnityEvent<SunlightRandomizer> { }
}