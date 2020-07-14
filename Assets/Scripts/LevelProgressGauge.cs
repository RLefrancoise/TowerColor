using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerColor
{
    /// <summary>
    /// Level progress gauge
    /// </summary>
    public class LevelProgressGauge : MonoBehaviour
    {
        /// <summary>
        /// Percentage
        /// </summary>
        private float _percentage;
        
        /// <summary>
        /// Fill gauge
        /// </summary>
        [SerializeField] private Image fill;
        
        /// <summary>
        /// Percentage text
        /// </summary>
        [SerializeField] private TMP_Text text;
        
        /// <summary>
        /// Round to int
        /// </summary>
        [SerializeField] private bool floorToInt = true;

        /// <summary>
        /// Current percentage between 0 and 1
        /// </summary>
        public float Percentage
        {
            get => _percentage;
            set
            {
                _percentage = value;

                var textValue = value * 100f;
                if (floorToInt) textValue = (int) textValue;
                
                text.text = $"{textValue}%";
                fill.fillAmount = _percentage;
            }
        }
    }
}