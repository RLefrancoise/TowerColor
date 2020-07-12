using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    [RequireComponent(typeof(BallBonusInstaller))]
    public class BallBonus : MonoBehaviour
    {
        private TriggerEvents _triggerEvents;
        private TMP_Text _valueText;
        private int _value;

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                _valueText.text = $"+{value}";
            }
        }
        
        public event Action<int> BonusAcquired;

        [Inject]
        public void Construct(TriggerEvents triggerEvents, TMP_Text valueText)
        {
            _triggerEvents = triggerEvents;
            _triggerEvents.TriggerEntered += TriggerEntered;

            _valueText = valueText;
        }

        private void TriggerEntered(Collider other)
        {
            if (other.gameObject.CompareTag("Brick"))
            {
                BonusAcquired?.Invoke(Value);
            }
        }
        
        public class Factory : PlaceholderFactory<BallBonus>
        {
        }
    }
    
    public class BallBonusFactory : IFactory<BallBonus>
    {
        private DiContainer _container;
        private GameData _gameData;
            
        public BallBonusFactory(DiContainer container, GameData gameData)
        {
            _container = container;
            _gameData = gameData;
        }
            
        public BallBonus Create()
        {
            return _container.InstantiatePrefab(_gameData.ballBonus).GetComponent<BallBonus>();
        }
    }
}