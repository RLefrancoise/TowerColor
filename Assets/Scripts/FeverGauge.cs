using System;
using TowerColor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerColor
{
    public class FeverGauge : MonoBehaviour
    {
        private int _maxCapacity;
        private int _currentCounter;
        private Sprite _defaultGauge;

        [SerializeField] private Image fill;
        [SerializeField] private Image gauge;
        [SerializeField] private Sprite whiteGaugeForRainbow;
        [SerializeField] private RainbowGraphic rainbowEffect;
        
        public event Action GaugeFilled;

        public int CurrentCounter
        {
            get => _currentCounter;
            set
            {
                _currentCounter = value;
                Refresh();
            }
        }
        
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

        public void Increment(int amount)
        {
            CurrentCounter += amount;
        }

        public void Empty()
        {
            CurrentCounter = 0;
        }

        public void Fill()
        {
            CurrentCounter = _maxCapacity;
        }

        public void SetRainbow(bool enable)
        {
            rainbowEffect.enabled = enable;
            gauge.sprite = enable ? whiteGaugeForRainbow : _defaultGauge;
            gauge.color = Color.white;
        }

        private void Refresh()
        {
            fill.fillAmount = (float) _currentCounter / _maxCapacity;
            if(fill.fillAmount >= 1f) GaugeFilled?.Invoke();
        }
    }
}