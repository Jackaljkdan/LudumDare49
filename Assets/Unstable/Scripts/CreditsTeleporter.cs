using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class CreditsTeleporter : Teleporter
    {
        #region Inspector



        #endregion

        [Inject(Id = "first")]
        private Transform first = null;

        [Inject(Id = "credits")]
        private Transform credits = null;

        [Inject(Id = "credits")]
        private Checkpoint checkpoint = null;

        [Inject(Id = "sun")]
        private Light sun = null;

        [Inject]
        private Worker worker = null;

        private void Start()
        {
            onTeleport.AddListener(OnTeleported);
            target = checkpoint.transform;
        }

        private void OnTeleported()
        {
            first.gameObject.SetActive(false);
            credits.gameObject.SetActive(true);

            player.prevCheckpoint = checkpoint;
            player.nextCheckpoint = checkpoint.next;

            worker.StartCoroutine(RotateSunCoroutine());
        }

        private IEnumerator RotateSunCoroutine()
        {
            while (true)
            {
                if (sun == null)
                    yield break;

                Vector3 euler = sun.transform.eulerAngles;
                euler.y += Time.deltaTime * 5;
                sun.transform.eulerAngles = euler;
                yield return null;
            }
        }
    }
    
    [Serializable]
    public class UnityEventCreditsTeleporter : UnityEvent<CreditsTeleporter> { }
}