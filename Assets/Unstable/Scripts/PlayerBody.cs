using JK.Sounds;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class PlayerBody : MonoBehaviour
    {
        #region Inspector

        public Transform upperBody;
        public Transform torso;
        public RandomClipsPlayer fallingSounds;

        public List<Color> colors = new List<Color>();

        public UnityEvent onStepComplete = new UnityEvent();

        #endregion

        public void OnStepCompleted()
        {
            onStepComplete.Invoke();
        }
        
        public void RandomizeColors()
        {
            var randomizers = GetComponentsInChildren<MaterialColorRandomizer>();

            Color color = randomizers[0].RandomizeColor();

            for (int i = 1; i < randomizers.Length; i++)
            {
                if (randomizers[i].name == "Hips")
                    randomizers[i].RandomizeColor();
                else
                    randomizers[i].SetColor(color);
            }
        }
    }

    [Serializable]
    public class UnityEventPlayerBody : UnityEvent<PlayerBody> { }
}