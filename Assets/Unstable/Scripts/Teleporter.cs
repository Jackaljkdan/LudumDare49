using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unstable.Interaction;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class Teleporter : TriggeredAction
    {
        #region Inspector

        public Transform target;

        public UnityEvent onTeleport = new UnityEvent();

        #endregion

        [Inject]
        protected Player player = null;

        [Inject]
        private Balance balance = null;

        [Inject]
        private Interference interference = null;

        [Inject]
        private Worker worker = null;

        protected override void PerformTriggeredAction()
        {
            worker.StartCoroutine(TeleportCoroutine());
        }

        private IEnumerator TeleportCoroutine()
        {
            balance.enabled = false;
            interference.StartInerferenceAndStopAfterDelay(0.5f);
            yield return new WaitForSeconds(0.5f);

            player.transform.position = target.position;
            player.transform.rotation = target.rotation;

            balance.balance = 0;
            balance.enabled = true;

            onTeleport.Invoke();
        }
    }
    
    [Serializable]
    public class UnityEventTeleporter : UnityEvent<Teleporter> { }
}