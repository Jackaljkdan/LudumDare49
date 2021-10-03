using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unstable.UI;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class Tutorial : MonoBehaviour
    {
        #region Inspector



        #endregion

        [Inject]
        private Balance balance = null;

        [Inject]
        private Player player = null;

        [Inject]
        private IntroTitles introTitles = null;

        [Inject(Id = "balance")]
        private CanvasGroup balanceCanvasGroup = null;

        private void Awake()
        {
            balance.enabled = false;
            player.enabled = false;

            if (player.TryGetComponent(out Accelerator accelerator))
                accelerator.enabled = false;

            introTitles.onFaded.AddListener(OnIntroFading);
        }

        private void OnIntroFading()
        {
            player.enabled = true;
            StartCoroutine(BalanceTutorialCoroutine());
        }

        private IEnumerator BalanceTutorialCoroutine()
        {
            yield return new WaitForSeconds(1);
            yield return balanceCanvasGroup.DOFade(1, duration: 0.3f).WaitForCompletion();

            yield return new WaitForSeconds(2);
            balance.enabled = true;

            yield return new WaitForSeconds(5);
            yield return balanceCanvasGroup.DOFade(0, duration: 0.3f).WaitForCompletion();
            balanceCanvasGroup.gameObject.SetActive(false);

            if (player.TryGetComponent(out Accelerator accelerator))
                accelerator.enabled = true;
        }
    }
    
    [Serializable]
    public class UnityEventTutorial : UnityEvent<Tutorial> { }
}