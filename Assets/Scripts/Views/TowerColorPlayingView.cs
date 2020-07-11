using System.Linq;
using Cinemachine;
using DG.Tweening;
using Framework.Game;
using Framework.UI;
using Framework.Views.Default;
using NaughtyAttributes;
using TowerColor.Views.Installers;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace TowerColor.Views
{
    [RequireComponent(typeof(TowerColorPlayingViewInstaller))]
    public class TowerColorPlayingView : DefaultPlayingView
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

        private PoppingMessageFactory _poppingMessageFactory;
        [ShowNonSerializedField] private Transform _colorChangeMessageAnchor;

        /// <summary>
        /// Remaining balls to fire before game over
        /// </summary>
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

        /// <summary>
        /// Balls that have been fired since the last color change
        /// </summary>
        private int BallsFiredSinceLastColorChange { get; set; }

        [Inject]
        public void Construct(
            ITouchSurface touchSurface, 
            IBallSpawner ballSpawner, 
            Camera playerCamera, 
            [Inject(Id = "GameCamera")] CinemachineVirtualCamera playerGameCamera,
            [Inject(Id = "LookAroundTowerCamera")] CinemachineVirtualCamera lookAroundTowerCamera,
            GameManager gameManager,
            GameData gameData,
            PoppingMessageFactory poppingMessageFactory,
            [Inject(Id = "ColorChangeMessageAnchor")] Transform colorChangeMessageAnchor)
        {
            _touchSurface = touchSurface;
            _ballSpawner = ballSpawner;
            
            _playerCamera = playerCamera;
            _playerGameCamera = playerGameCamera;
            _lookAroundTowerCamera = lookAroundTowerCamera;
            
            _gameManager = gameManager;
            _gameData = gameData;

            _poppingMessageFactory = poppingMessageFactory;
            _colorChangeMessageAnchor = colorChangeMessageAnchor;
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
            RemainingBalls = (int) _gameManager.LevelManager.GetCurveValue(_gameData.numberOfBallsByLevel);
            
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
            
            //Show ball spawner
            _ballSpawner.Activate(true);
            
            //Spawn ball
            _ball = _ballSpawner.SpawnBall();
            
            //Set ball color
            _ball.Color = availableColors[Random.Range(0, availableColors.Count())];
            
            _ball.TouchedBrick += OnBallTouchedBrick;
        }

        /// <summary>
        /// Trigger color change feature
        /// </summary>
        private async UniTask TriggerColorChange()
        {
            Debug.Log("Color change");

            //Reset balls counter
            BallsFiredSinceLastColorChange = 0;
            
            //Show color change message and wait until it disappears
            var colorChangeMessage = _poppingMessageFactory.Create(_gameData.colorChangeMessage);
            colorChangeMessage.AttachTo(_colorChangeMessageAnchor);
            
            var popOver = false;
            colorChangeMessage.PopOver += () => popOver = true;
            await UniTask.WaitUntil(() => popOver);
            
            //Shuffle tower colors
            await _gameManager.Tower.ShuffleColors(true, true, false);
        }

        /// <summary>
        /// When a ball has touched a brick, we destroy it, as well as the brick and adjacent bricks with same color
        /// </summary>
        /// <param name="brick">Brick being touched</param>
        private async void OnBallTouchedBrick(Brick brick)
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
            
            //Check if we won
            if (!CheckWinCondition())
            {
                BallsFiredSinceLastColorChange++;
                
                if (BallsFiredSinceLastColorChange == (int) _gameManager.LevelManager.GetCurveValue(_gameData.ballsToFireToTriggerColorChange))
                {
                    await TriggerColorChange();
                }
                
                //Decrease remaining balls
                RemainingBalls--;

                //If we still have balls, spawn a new one
                if (RemainingBalls > 0)
                {
                    SpawnNewBall();
                }
            }
        }

        /// <summary>
        /// Check if we won the game
        /// </summary>
        private bool CheckWinCondition()
        {
            //There is no more bricks
            if (!_gameManager.Tower.Steps.SelectMany(s => s.Bricks).Any())
            {
                _gameManager.ChangeState(GameState.Win);
                return true;
            }
            if (_gameManager.Tower.CurrentStep <= _gameData.minimumTowerStepToWin) // Current step is below required step
            {
                var lastStepDestroyRatio = _gameManager.Tower.Steps[0].DestroyedRatio;
                if (lastStepDestroyRatio >= _gameData.destroyRatioOfBottomTowerToWin) //And bottom tower step destroy ratio is above required one
                {
                    _gameManager.ChangeState(GameState.Win);
                    return true;
                }
            }

            return false;
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
                
                //Detach ball from spawner
                _ball.transform.SetParent(null);
                
                //Fire ball
                _ball.FireTo(brick, hit.point, hit.normal);
                
                //Hide ball spawner
                _ballSpawner.Activate(false);
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