using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Unstable
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class Interference : MonoBehaviour
    {
        #region Inspector

        public float fadeSeconds = 0.3f;

        public UnityEvent onStart = new UnityEvent();
        public UnityEvent onEnd = new UnityEvent();

        #endregion

        [Inject]
        private void Inject()
        {
            gameObject.SetActive(false);
        }

        public void StartInterference()
        {
            gameObject.SetActive(true);

            var group = GetComponent<CanvasGroup>();
            group.alpha = 0;
            group.DOFade(1, fadeSeconds);

            onStart.Invoke();
        }

        public void StartInerferenceAndStopAfterDelay(float delaySeconds)
        {
            StartInterference();
            Invoke(nameof(StopInterference), delaySeconds);
        }

        public void StopInterference()
        {
            StartCoroutine(StopCoroutine());
        }

        private IEnumerator StopCoroutine()
        {
            onEnd.Invoke();

            yield return GetComponent<CanvasGroup>().DOFade(0, fadeSeconds).WaitForCompletion();
            gameObject.SetActive(false);
        }
    }
    
    [Serializable]
    public class UnityEventInterference : UnityEvent<Interference> { }
}