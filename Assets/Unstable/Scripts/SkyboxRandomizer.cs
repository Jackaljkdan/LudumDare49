using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class SkyboxRandomizer : MonoBehaviour
    {
        #region Inspector

        public List<Material> skyboxes = new List<Material>();

        #endregion

        [Inject]
        private Interference interference = null;

        private void Start()
        {
            interference.onEnd.AddListener(OnInterferenceEnd);
        }

        private void OnInterferenceEnd()
        {
            int randomIndex = UnityEngine.Random.Range(0, skyboxes.Count);
            Material randomSkybox = skyboxes[randomIndex];

            RenderSettings.skybox = randomSkybox;
        }
    }
    
    [Serializable]
    public class UnityEventSkyboxRandomizer : UnityEvent<SkyboxRandomizer> { }
}