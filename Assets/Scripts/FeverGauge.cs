using System;
using TowerColor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerColor
{
    /// <summary>
    /// Generic fever gauge with rainbow effect
    /// </summary>
    public class FeverGauge : MonoBehaviour
    {
        /// <summary>
        /// Max capacity
        /// </summary>
        private int _maxCapacity;
        
        /// <summary>
        /// Current counter
        /// </summary>
        private int _currentCounter;
        
        /// <summary>
        /// Default gauge sprite
        /// </summary>
        private Sprite _defaultGauge;

        /// <summary>
        /// Fill mask
        /// </summary>
        [SerializeField] private Image fill;
        
        /// <summary>
        /// Gauge image
        /// </summary>
        [SerializeField] private Image gauge;
        
        /// <summary>
        /// For rainbow, we need a white sprite
        /// </summary>
        [SerializeField] private Sprite whiteGaugeForRainbow;
        
        /// <summary>
        /// Rainbow effect
        /// </summary>
        [SerializeField] private RainbowGraphic rainbowEffect;
        
        /// <summary>
        /// When gauge filled
        /// </summary>
        public event Action GaugeFilled;

        /// <summary>
        /// Current counter
        /// </summary>
        public int CurrentCounter
        {
            get => _currentCounter;
            set
            {
                _currentCounter = value;
                Refresh();
            }
        }
        
        /// <summary>
        /// Max capacity
        /// </summary>
        public int MaxCapacity
        {
            get => _maxCapacity;
            set
            {
                _maxCapacity = value;
                Refresh();
            }
        }

        private void Awake()
        {
            _defaultGauge = gauge.sprite;
            Empty();
        }

        /// <summary>
        /// Increment counter
        /// </summary>
        /// <param name="amount">Amount</param>
        public void Increment(int amount)
        {
            CurrentCounter += amount;
        }

        /// <summary>
        /// Empty gauge
        /// </summary>
        public void Empty()
        {
            CurrentCounter = 0;
        }

        /// <summary>
        /// Fill gauge
        /// </summary>
        public void Fill()
        {
            CurrentCounter = _maxCapacity;
        }

        /// <summary>
        /// Enable rainbow effect
        /// </summary>
        /// <param name="enable">Enable</param>
        public void SetRainbow(bool enable)
        {
            rainbowEffect.enabled = enable;
            gauge.sprite = enable ? whiteGaugeForRainbow : _defaultGauge;
            gauge.color = Color.white;
        }

        /// <summary>
        /// Refresh state
        /// </summary>
        private void Refresh()
        {
            fill.fillAmount = (float) _currentCounter / _maxCapacity;
            if(fill.fillAmount >= 1f) GaugeFilled?.Invoke();
        }
    }
}