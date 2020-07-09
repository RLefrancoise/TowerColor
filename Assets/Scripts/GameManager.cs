using System.Linq;
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    /// <summary>
    /// The game manager handles the logic of the game
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Fields
        
        private GameData _gameData;
        private SaveData _saveData;
        
        private TowerSpawner _towerSpawner;
        private BallSpawner _ballSpawner;

        private Camera _playerCamera;
        private CinemachineVirtualCamera _playerGameCamera;
        
        private TouchSurface _touchSurface;
        
        private Tower _tower;
        private Ball _ball;
        private GameObject _playerCameraFocusPoint;
        
        #endregion

        #region Properties
        
        [ShowNativeProperty] public bool IsGameStarted { get; private set; }
        
        [ShowNativeProperty] public int CurrentLevel { get; private set; }
        
        [ShowNativeProperty] public int RemainingBalls { get; private set; }
        
        #endregion

        [Inject]
        public void Construct(
            GameData gameData,
            SaveData saveData,
            TowerSpawner towerSpawner, 
            BallSpawner ballSpawner,
            Camera playerCamera,
            [Inject(Id = "GameCamera")] CinemachineVirtualCamera playerGameCamera, 
            TouchSurface touchSurface)
        {
            _gameData = gameData;
            _saveData = saveData;
            
            _towerSpawner = towerSpawner;
            _ballSpawner = ballSpawner;
            
            _playerCamera = playerCamera;
            _playerGameCamera = playerGameCamera;
            
            _touchSurface = touchSurface;
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
            
            IsGameStarted = true;

            //Get number of balls for this level
            RemainingBalls = (int) _gameData.numberOfBallsByLevel.Evaluate((CurrentLevel - 1) / 100f);
            
            //Spawn a new ball
            SpawnNewBall();
            
            //Set tower current step to the top one
            _tower.SetCurrentStep(_tower.Steps.Count - 1);
        }
        
        #endregion

        #region MonoBehaviours
        
        private void Start()
        {
            //Init current level with the level from the save data
            CurrentLevel = _saveData.CurrentLevel;
            
            //Create tower
            _tower = _towerSpawner.SpawnRandomTower(CurrentLevel);
            _tower.Init(CurrentLevel);
            _tower.EnablePhysics(false);
            
            _tower.CurrentStepChanged += OnTowerCurrentStepChanged;
            
            //Listen touch surface
            _touchSurface.Touched += OnPlayerTouch;
            _touchSurface.Dragging += OnPlayerDrag;
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// Spawn a new ball
        /// </summary>
        private void SpawnNewBall()
        {
            var availableColors = _tower.AvailableColors.ToList();
            if (availableColors.Count == 0)
            {
                Debug.LogError("Tower has no available colors");
                return;
            }
            
            _ball = _ballSpawner.SpawnBall();
            _ball.Color = availableColors[Random.Range(0, availableColors.Count())];
            
            _ball.TouchedBrick += OnBallTouchedBrick;
        }

        /// <summary>
        /// When a ball has touched a brick, we destroy it, as well as the brick and adjacent bricks with same color
        /// </summary>
        /// <param name="brick">Brick being touched</param>
        private void OnBallTouchedBrick(Brick brick)
        {
            //Destroy ball
            Destroy(_ball.gameObject);
            _ball = null;
            
            Debug.LogFormat("Ball has touched brick {0}", brick.name);
            
            //Destroy brick
            var bricksToDestroy = _tower.GetBricksWithSameColor(brick);
            foreach (var b in bricksToDestroy)
            {
                Destroy(b.gameObject);
            }
            
            //Decrease remaining balls
            RemainingBalls--;

            //If we still have balls, spawn a new one
            if (RemainingBalls > 0)
            {
                SpawnNewBall();
            }
        }

        /// <summary>
        /// When player touch the screen to fire a ball
        /// </summary>
        /// <param name="touchPosition">Screen position</param>
        private void OnPlayerTouch(Vector2 touchPosition)
        {
            if (!IsGameStarted) return;
            
            Debug.LogFormat("Player touched screen at position {0}", touchPosition);

            if (!_ball)
            {
                Debug.Log("Player has no ball, don't fire");
                return;
            }
            
            var ray = _playerCamera.ScreenPointToRay(new Vector3(touchPosition.x * Screen.width, touchPosition.y * Screen.height));
            if (Physics.Raycast(ray, out var hit, 100f, LayerMask.GetMask("Brick")))
            {
                var brick = hit.collider.GetComponentInParent<Brick>();

                if (!_tower.IsBrickTargetable(brick))
                {
                    Debug.LogFormat("Brick {0} is not targetable", brick.name);
                    return;
                }

                // Check brick is still in place, falling bricks must not be a target
                /*if (!brick.IsStillInPlace)
                {
                    Debug.LogFormat("Brick {0} is not still in place, ignore it", brick.name);
                    return;
                }*/
                
                //Check if brick has same color as the ball
                if(brick.Color == _ball.Color)
                    _ball.FireTo(brick);
                else
                    Debug.Log("Brick are ball colors are different");
            }
        }
        
        /// <summary>
        /// When player is dragging the screen
        /// </summary>
        /// <param name="dragDelta">Drag delta</param>
        private void OnPlayerDrag(Vector2 dragDelta)
        {
            _playerGameCamera.transform.RotateAround(_tower.transform.position, _tower.transform.up, dragDelta.x * _gameData.towerRotateSpeed);
        }
        
        /// <summary>
        /// When tower current step is changed, we update camera etc...
        /// </summary>
        /// <param name="step">Tower step</param>
        private void OnTowerCurrentStepChanged(int step)
        {
            //Create new focus point
            if(_playerCameraFocusPoint) Destroy(_playerCameraFocusPoint);
            
            _playerCameraFocusPoint = new GameObject("FocusPoint");
            _playerCameraFocusPoint.transform.SetParent(transform, false);
            _playerCameraFocusPoint.transform.position = _tower.CurrentSubTowerFocusPoint.position;
            
            //Update game camera focus point
            var newPosition = new Vector3(
                _playerGameCamera.transform.position.x, 
                _playerCameraFocusPoint.transform.position.y, 
                _playerGameCamera.transform.position.z);
            _playerGameCamera.transform.DOMove(newPosition, _gameData.goToStepCameraMovementDuration);
            _playerGameCamera.LookAt = _playerCameraFocusPoint.transform;
        }
        
        #endregion
    }
}