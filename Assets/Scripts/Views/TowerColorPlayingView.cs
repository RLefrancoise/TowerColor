using System.Linq;
using Cinemachine;
using DG.Tweening;
using Framework.Game;
using Framework.Views;
using NaughtyAttributes;
using TowerColor.Views.Installers;
using UnityEngine;
using Zenject;

namespace TowerColor.Views
{
    [RequireComponent(typeof(TowerColorPlayingViewInstaller))]
    public class TowerColorPlayingView : PlayingView
    {
        private int _remainingBalls;
        
        private ITouchSurface _touchSurface;
        private IBallSpawner _ballSpawner;
        
        private Camera _playerCamera;
        private CinemachineVirtualCamera _playerGameCamera;
        private CinemachineVirtualCamera _lookAroundTowerCamera;
        
        private GameManager _gameManager;
        private GameData _gameData;
        
        private Ball _ball;
        private GameObject _playerCameraFocusPoint;

        [ShowNativeProperty] public int RemainingBalls
        {
            get => _remainingBalls;
            set
            {
                _remainingBalls = value;
                _ballSpawner.SetRemainingBalls(value);
                
                //If no more balls, game over
                if (_remainingBalls <= 0)
                {
                    _gameManager.ChangeState(GameState.GameOver);
                }
            }
        }
        
        [Inject]
        public void Construct(
            ITouchSurface touchSurface, 
            IBallSpawner ballSpawner, 
            Camera playerCamera, 
            [Inject(Id = "GameCamera")] CinemachineVirtualCamera playerGameCamera,
            [Inject(Id = "LookAroundTowerCamera")] CinemachineVirtualCamera lookAroundTowerCamera,
            GameManager gameManager,
            GameData gameData)
        {
            _touchSurface = touchSurface;
            _ballSpawner = ballSpawner;
            
            _playerCamera = playerCamera;
            _playerGameCamera = playerGameCamera;
            _lookAroundTowerCamera = lookAroundTowerCamera;
            
            _gameManager = gameManager;
            _gameData = gameData;
        }

        protected override void OnShow()
        {
            base.OnShow();
            
            //Enable player game camera
            _playerGameCamera.gameObject.SetActive(true);

            //The game camera must be at the same place as the look around tower camera to avoid glitches
            _playerGameCamera.transform.position = _lookAroundTowerCamera.transform.position;
            
            //Listen touch surface
            _touchSurface.Touched += OnPlayerTouch;
            _touchSurface.Dragging += OnPlayerDrag;
            
            //Listen tower step changed
            _gameManager.Tower.CurrentStepChanged += OnTowerCurrentStepChanged;
            
            //Set tower current step to the top one
            _gameManager.Tower.SetCurrentStep(_gameManager.Tower.Steps.Count - 1);
            
            //Get number of balls for this level
            RemainingBalls = (int) _gameData.numberOfBallsByLevel.Evaluate((_gameManager.LevelManager.CurrentLevel - 1) / 100f);
            
            //Enable ball spawner
            _ballSpawner.Activate(true);
            
            //Start game
            StartGame();
        }

        protected override void OnHide()
        {
            base.OnHide();

            //Disable ball spawner
            _ballSpawner.Activate(false);
            
            //Disable player game camera
            _playerGameCamera.gameObject.SetActive(false);
            
            //Stop listen touch surface
            _touchSurface.Touched -= OnPlayerTouch;
            _touchSurface.Dragging -= OnPlayerDrag;
            
            //Stop listen tower state changed
            if(_gameManager.Tower) _gameManager.Tower.CurrentStepChanged -= OnTowerCurrentStepChanged;
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        private void StartGame()
        {
            SpawnNewBall();
        }
        
        /// <summary>
        /// Spawn a new ball
        /// </summary>
        private void SpawnNewBall()
        {
            var availableColors = _gameManager.Tower.AvailableColors.ToList();
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
            if (_ball.Color == brick.Color)
            {
                //Destroy ball
                Destroy(_ball.gameObject);
                _ball = null;
                
                Debug.LogFormat("Ball has touched brick {0}", brick.name);
                
                //Destroy brick
                var bricksToDestroy = _gameManager.Tower.GetBricksWithSameColor(brick);
                foreach (var b in bricksToDestroy)
                {
                    Destroy(b.gameObject);
                }
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

                if (!_gameManager.Tower.IsBrickTargetable(brick))
                {
                    Debug.LogFormat("Brick {0} is not targetable", brick.name);
                    return;
                }
                
                _ball.FireTo(brick, hit.point, hit.normal);
            }
        }
        
        /// <summary>
        /// When player is dragging the screen
        /// </summary>
        /// <param name="dragDelta">Drag delta</param>
        private void OnPlayerDrag(Vector2 dragDelta)
        {
            //No drag if ball is being fired
            if(_ball && _ball.IsFiring) return;
            
            _playerGameCamera.transform.RotateAround(
                _gameManager.Tower.transform.position, 
                _gameManager.Tower.transform.up, 
                dragDelta.x * _gameData.towerRotateSpeed);
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
            _playerCameraFocusPoint.transform.position = _gameManager.Tower.CurrentSubTowerFocusPoint.position;
            
            //Update game camera focus point
            var newPosition = new Vector3(
                _playerGameCamera.transform.position.x, 
                _playerCameraFocusPoint.transform.position.y, 
                _playerGameCamera.transform.position.z);
            _playerGameCamera.transform.DOMove(newPosition, _gameData.goToStepCameraMovementDuration);
            _playerGameCamera.LookAt = _playerCameraFocusPoint.transform;
        }
    }
}