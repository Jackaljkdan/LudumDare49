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
    public class Respawner : MonoBehaviour
    {
        #region Inspector

        public float seconds = 2;

        [SerializeField]
        private PlayerBody bodyPrefab = null;

        [SerializeField]
        private Transform body = null;

        #endregion

        [Inject]
        private Balance balance = null;

        private void Start()
        {
            balance.onBalanceLost.AddListener(OnBalanceLost);
        }

        private void OnBalanceLost()
        {
            StartCoroutine(RespawnCoroutine());
        }

        private IEnumerator RespawnCoroutine()
        {
            yield return new WaitForSeconds(seconds);

            PlayerBody newBody = Instantiate(bodyPrefab, body.parent);

            foreach (var foot in newBody.GetComponentsInChildren<Foot>())
                foot.balance = balance;

            GetComponent<Player>().Respawn(newBody);
            balance.Respawn(newBody);
        }
    }
    
    [Serializable]
    public class UnityEventRespawner : UnityEvent<Respawner> { }
}