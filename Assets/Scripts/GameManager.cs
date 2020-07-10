using Framework.Game;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    /// <summary>
    /// The game manager handles the logic of the game
    /// </summary>
    public class GameManager : GameManagerBase
    {
        #region Fields

        private TowerSpawner _towerSpawner;
        
        private Tower _tower;
        private int _remainingBalls;
        private GameObject _playerCameraFocusPoint;
        
        #endregion

        #region Properties

        public Tower Tower => _tower;
        
        [ShowNativeProperty] public bool IsGameStarted => CurrentState == GameState.Playing;
        [ShowNativeProperty] public int RemainingBalls
        {
            get => _remainingBalls;
            set
            {
                _remainingBalls = value;
                
                //If no more balls, game over
                if (_remainingBalls <= 0)
                {
                    ChangeState(GameState.GameOver);
                }
            }
        }
        
        #endregion

        [Inject]
        public void Construct(
            TowerSpawner towerSpawner)
        {
            _towerSpawner = towerSpawner;
        }

        #region Public Methods
        
        /// <summary>
        /// Start a new game
        /// </summary>
        [Button("Start game")]
        public void StartGame()
        {
            if (IsGameStarted)
            {
                Debug.LogError("Game is already started");
                return;
            }
            
            ChangeState(GameState.Start);
        }
        
        #endregion

        #region MonoBehaviours
        
        protected override void Start()
        {
            base.Start();

            //Create tower
            _tower = _towerSpawner.SpawnRandomTower(LevelManager.CurrentLevel);
            _tower.Init(LevelManager.CurrentLevel);
            _tower.EnablePhysics(false);
        }
        
        #endregion
        
        #if UNITY_EDITOR

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
        }

        #endif
    }
}