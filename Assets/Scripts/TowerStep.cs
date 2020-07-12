using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Framework.Game;
using NaughtyAttributes;
using TowerColor.Views;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class TowerStep : MonoBehaviour
    {
        private int _bricksCountAtStart;
        private GameManager _gameManager;
        private GameData _gameData;
        
        [SerializeField] private List<Brick> bricks;

        public ReadOnlyCollection<Brick> Bricks => bricks.AsReadOnly();

        public float Height => bricks[0].Height;

        [ShowNativeProperty] public bool IsActivated { get; private set; } = true;

        [ShowNativeProperty] public bool IsFullyDestroyed { get; private set; }

        [ShowNativeProperty] public float DestroyedRatio
        {
            get
            {
                var missingBricks = _bricksCountAtStart - bricks.Count;
                var bricksMoved = bricks.Count(b => !b.IsStillInPlace);

                return (missingBricks + bricksMoved) / (float) _bricksCountAtStart;
            }
        }

        public event Action FullyDestroyed;

        [Inject]
        public void Construct(GameManager gameManager, GameData gameData)
        {
            _gameManager = gameManager;
            _gameData = gameData;
        }
        
        private void Start()
        {
            _bricksCountAtStart = bricks.Count;
            
            foreach (var brick in bricks)
            {
                brick.Destroyed += OnBrickDestroyed;
            }
        }

        private void Update()
        {
            if(_gameManager.CurrentState != GameState.Playing) return;
            if(IsFullyDestroyed) return;

            if (DestroyedRatio >= _gameData.towerStepDestroyedMinimumRatio)
            {
                Debug.LogFormat("Step {0} fully destroyed", name);
                IsFullyDestroyed = true;
                FullyDestroyed?.Invoke();
            }
        }
        
        public void EnablePhysics(bool enable)
        {
            foreach (var brick in bricks)
            {
                brick.PhysicsEnabled = enable;
            }
        }

        public void ActivateStep(bool activate)
        {
            IsActivated = activate;
            
            foreach(var brick in bricks)
                brick.SetActivated(activate);
        }
        
        private void OnBrickDestroyed(Brick brick)
        {
            bricks.Remove(brick);
        }
    }
}