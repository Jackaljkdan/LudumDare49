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
        public float interferenceSeconds = 0.5f;

        [SerializeField]
        private PlayerBody bodyPrefab = null;

        [SerializeField]
        private Transform body = null;

        #endregion

        [Inject]
        private Balance balance = null;

        [Inject]
        private Interference interference = null;

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
            yield return new WaitForSeconds(seconds - interferenceSeconds);
            interference.StartInerferenceAndStopAfterDelay(interferenceSeconds);
            yield return new WaitForSeconds(interferenceSeconds);

            PlayerBody newBody = Instantiate(bodyPrefab, body.parent);

            foreach (var foot in newBody.GetComponentsInChildren<Foot>())
                foot.balance = balance;

            GetComponent<Player>().Respawn(newBody);
            balance.Respawn(newBody);

            Destroy(body.gameObject);
            body = newBody.transform;
        }
    }
    
    [Serializable]
    public class UnityEventRespawner : UnityEvent<Respawner> { }
}