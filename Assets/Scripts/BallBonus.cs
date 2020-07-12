using System;
using TMPro;
using UnityEngine;
using UnityQuery;
using Zenject;

namespace TowerColor
{
    [RequireComponent(typeof(BallBonusInstaller))]
    public class BallBonus : MonoBehaviour
    {
        private GameManager _gameManager;
        private CollisionEvents _triggerEvents;
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
        
        public float RotateSpeed { get; set; }
        
        public event Action<int> BonusAcquired;

        private void Update()
        {
            transform.RotateAround(_gameManager.Tower.transform.position, _gameManager.Tower.transform.up, RotateSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation((transform.position - _gameManager.Tower.transform.position.WithY(transform.position.y)).normalized);
        }
        
        [Inject]
        public void Construct(GameManager gameManager, CollisionEvents triggerEvents, TMP_Text valueText)
        {
            _gameManager = gameManager;
            _triggerEvents = triggerEvents;
            _triggerEvents.CollisionEnter += CollisionEntered;

            _valueText = valueText;
        }

        private void CollisionEntered(Collision other)
        {
            Debug.LogFormat("{0} touched ball bonus {1}", other.collider.name, name);
            
            if (other.gameObject.CompareTag("Brick"))
            {
                var brick = other.collider.GetComponentInParent<Brick>();
                brick.Break();

                BonusAcquired?.Invoke(Value);
            }
        }
        
        public class Factory : PlaceholderFactory<BallBonus>
        {
        }
    }
    
    public class BallBonusFactory : IFactory<BallBonus>
    {
        private readonly DiContainer _container;
        private readonly GameData _gameData;
            
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