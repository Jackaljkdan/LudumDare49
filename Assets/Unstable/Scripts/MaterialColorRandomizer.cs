using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unstable
{
    [DisallowMultipleComponent]
    public class MaterialColorRandomizer : MonoBehaviour
    {
        #region Inspector

        public List<Color> colors = new List<Color>();

        #endregion

        public Color RandomizeColor()
        {
            int randomIndex = UnityEngine.Random.Range(0, colors.Count);
            Color randomColor = colors[randomIndex];

            SetColor(randomColor);

            return randomColor;
        }

        public void SetColor(Color color)
        {
            GetComponent<MeshRenderer>().material.color = color;
        }
    }
    
    [Serializable]
    public class UnityEventMaterialColorRandomizer : UnityEvent<MaterialColorRandomizer> { }
}