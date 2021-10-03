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
    public class RestartAfterCredits : Unstable.Interaction.TriggeredAction
    {
        #region Inspector



        #endregion

        [Inject]
        private Interference interference = null;

        [Inject]
        private Worker worker = null;

        [Inject]
        private Balance balance = null;

        protected override void PerformTriggeredAction()
        {
            worker.StartCoroutine(RestartCoroutine());
        }

        private IEnumerator RestartCoroutine()
        {
            balance.enabled = false;
            interference.StartInterference();
            yield return new WaitForSeconds(0.5f);

            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Game");
        }
    }
    
    [Serializable]
    public class UnityEventRestartAfterCredits : UnityEvent<RestartAfterCredits> { }
}