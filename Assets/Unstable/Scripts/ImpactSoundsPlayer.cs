using JK.Sounds;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unstable
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RandomClipsPlayer))]
    public class ImpactSoundsPlayer : MonoBehaviour
    {
        #region Inspector

        

        #endregion

        private void Start()
        {
            foreach (var detector in GetComponentsInChildren<HitDetector>())
                detector.onHit.AddListener(OnHit);
        }

        private void OnHit(Collider arg0, Collision arg1)
        {
            GetComponent<RandomClipsPlayer>().PlayRandom();
        }
    }
    
    [Serializable]
    public class UnityEventImpactSoundsPlayer : UnityEvent<ImpactSoundsPlayer> { }
}