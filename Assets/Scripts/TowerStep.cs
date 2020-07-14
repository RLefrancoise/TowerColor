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
    /// <summary>
    /// A tower step
    /// </summary>
    public class TowerStep : MonoBehaviour
    {
        /// <summary>
        /// State is initialized ?
        /// </summary>
        [ShowNonSerializedField] private bool _hasStateInitialized;
        
        /// <summary>
        /// Number of bricks at start
        /// </summary>
        [ShowNonSerializedField] private int _bricksCountAtStart;
        
        /// <summary>
        /// Game manager
        /// </summary>
        private GameManager _gameManager;
        
        /// <summary>
        /// Game data
        /// </summary>
        private GameData _gameData;
        
        /// <summary>
        /// Bricks
        /// </summary>
        [SerializeField] private List<Brick> bricks;

        /// <summary>
        /// Bricks
        /// </summary>
        public ReadOnlyCollection<Brick> Bricks => bricks.AsReadOnly();

        /// <summary>
        /// Step height
        /// </summary>
        public float Height => bricks[0].Height;

        /// <summary>
        /// Is step activated ?
        /// </summary>
        [ShowNativeProperty] public bool IsActivated { get; private set; } = true;

        /// <summary>
        /// Is step fully destroyed ?
        /// </summary>
        [ShowNativeProperty] public bool IsFullyDestroyed { get; private set; }

        /// <summary>
        /// Destroyed ratio
        /// </summary>
        [ShowNativeProperty] public float DestroyedRatio
        {
            get
            {
                var missingBricks = _bricksCountAtStart - bricks.Count;
                var bricksMoved = bricks.Count(b => !b.IsStillInPlace);

                return (missingBricks + bricksMoved) / (float) _bricksCountAtStart;
            }
        }

        /// <summary>
        /// When fully destroyed
        /// </summary>
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
            if(!_hasStateInitialized) return;
            if(IsFullyDestroyed) return;

            if (DestroyedRatio >= _gameData.towerStepDestroyedMinimumRatio)
            {
                Debug.LogFormat("Step {0} fully destroyed", name);
                IsFullyDestroyed = true;
                FullyDestroyed?.Invoke();
            }
        }

        /// <summary>
        /// Init state
        /// </summary>
        public void InitializeState()
        {
            foreach (var brick in bricks)
            {
                brick.InitializeState();
            }

            _hasStateInitialized = true;
        }
        
        /// <summary>
        /// Enable physics
        /// </summary>
        /// <param name="enable">Enable or disable</param>
        public void EnablePhysics(bool enable)
        {
            foreach (var brick in bricks)
            {
                brick.PhysicsEnabled = enable;
            }
        }

        /// <summary>
        /// Activate step
        /// </summary>
        /// <param name="activate">Activate</param>
        /// <param name="force">Force activate</param>
        public void ActivateStep(bool activate, bool force = false)
        {
            IsActivated = activate;
            
            foreach(var brick in bricks)
                brick.SetActivated(activate, force);
        }
        
        /// <summary>
        /// When brick destroyed, remove it from list
        /// </summary>
        /// <param name="brick">Destroyed brick</param>
        private void OnBrickDestroyed(Brick brick)
        {
            bricks.Remove(brick);
        }
    }
}