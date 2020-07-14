using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerColor
{
    public class LevelProgressGauge : MonoBehaviour
    {
        private float _percentage;
        
        [SerializeField] private Image fill;
        [SerializeField] private TMP_Text text;
        [SerializeField] private bool floorToInt = true;

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