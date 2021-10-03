using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Unstable.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class IntroTitles : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private float fadeSeconds = 1;

        public UnityEvent onFaded = new UnityEvent();

        #endregion

        [Inject(Id = "music")]
        private AudioSource music = null;

        [Inject]
        private void Inject()
        {
            music.playOnAwake = false;

            if (Mathf.Approximately(music.time, 0))
                music.Stop();
        }

        private void Update()
        {
            if (UnityEngine.Input.anyKeyDown)
            {
                StartCoroutine(FadeCoroutine());
                enabled = false;
            }
        }

        private IEnumerator FadeCoroutine()
        {
            yield return GetComponent<CanvasGroup>().DOFade(0, duration: fadeSeconds).WaitForCompletion();
            gameObject.SetActive(false);

            if (!music.isPlaying)
                music.Play();

            onFaded.Invoke();
        }
    }
}