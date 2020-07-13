using System;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Framework.Game;
using Framework.UI;
using Framework.Views.Default;
using NaughtyAttributes;
using TowerColor.UI;
using TowerColor.Views.Installers;
using UniRx.Async;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

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
        private TweenerCore<Vector3, Vector3, VectorOptions> _movePlayerGameCameraTween;
        
        private GameManager _gameManager;
        private GameData _gameData;
        
        private Ball _ball;
        
        private GameObject _playerCameraFocusPoint;

        private PoppingMessageFactory _poppingMessageFactory;
        private BallGainedMessage.Factory _ballGainedMessageFactory;
        
        private Transform _colorChangeMessageAnchor;
        private Transform _feedbackMessageAnchor;
        private Transform _ballGainedMessageAnchor;

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
            BallGainedMessage.Factory ballGainedMessageFactory,
            [Inject(Id = "ColorChangeMessageAnchor")] Transform colorChangeMessageAnchor,
            [Inject(Id = "FeedbackMessageAnchor")] Transform feedbackMessageAnchor,
            [Inject(Id = "BallGainedMessageAnchor")] Transform ballGainedMessageAnchor)
        {
            _touchSurface = touchSurface;
            _ballSpawner = ballSpawner;
            
            _playerCamera = playerCamera;
            _playerGameCamera = playerGameCamera;
            _lookAroundTowerCamera = lookAroundTowerCamera;
            
            _gameManager = gameManager;
            _gameData = gameData;
            
            _poppingMessageFactory = poppingMessageFactory;
            _ballGainedMessageFactory = ballGainedMessageFactory;
            
            _colorChangeMessageAnchor = colorChangeMessageAnchor;
            _feedbackMessageAnchor = feedbackMessageAnchor;
            _ballGainedMessageAnchor = ballGainedMessageAnchor;
        }

        protected override async void OnShow()
        {
            base.OnShow();
            
            //Enable player game camera
            _playerGameCamera.gameObject.SetActive(true);

            //The game camera must be at the same place as the look around tower camera to avoid glitches
            _playerGameCamera.transform.position = _lookAroundTowerCamera.transform.position;
            
            //Listen touch surface
            _touchSurface.Touched += OnPlayerTouch;
            _touchSurface.Dragging += OnPlayerDrag;
            
            //Listen ball bonuses
            foreach (var bonus in _gameManager.BallBonuses)
            {
                bonus.BonusAcquired += OnBallBonusAcquired;
            }
            
            //Listen tower step changed
            _gameManager.Tower.CurrentStepChanged += OnTowerCurrentStepChanged;
            
            //Set tower current step to the top one
            await _gameManager.Tower.SetCurrentStep(_gameManager.Tower.Steps.Count - 1, false, true);
            
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
            
            //Stop listen ball bonuses
            foreach (var bonus in _gameManager.BallBonuses)
            {
                bonus.BonusAcquired -= OnBallBonusAcquired;
            }
            
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
                
                //Destroy brick
                var bricksToDestroy = _gameManager.Tower.GetBricksWithSameColor(brick);
                foreach (var b in bricksToDestroy)
                {
                    b.Break();
                    await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
                }

                GameObject feedbackMessage = null;
                int ballGained = 0;
                
                //Show message feedback according to number of bricks destroyed
                if (bricksToDestroy.Count >= _gameData.insaneMessageBricksCount)
                {
                    feedbackMessage = _gameData.insaneMessage;
                    ballGained = _gameData.ballsGainedAfterInsaneMessage;
                }
                else if (bricksToDestroy.Count >= _gameData.greatMessageBricksCount)
                {
                    feedbackMessage = _gameData.greatMessage;
                    ballGained = _gameData.ballsGainedAfterGreatMessage;
                }
                else if (bricksToDestroy.Count >= _gameData.goodMessageBricksCount)
                {
                    feedbackMessage = _gameData.goodMessage;
                    ballGained = _gameData.ballsGainedAfterGoodMessage;
                }

                if (feedbackMessage)
                {
                    //If we gained one or more balls, display it
                    if (ballGained > 0)
                    {
                        var ballGainedMessage = _ballGainedMessageFactory.Create(_gameData.ballGainedMessage);
                        ballGainedMessage.BallsGained = ballGained;
                        ballGainedMessage.AttachTo(_ballGainedMessageAnchor);

                        //And add balls to remaining balls
                        RemainingBalls += ballGained;
                    }
                    
                    var feedback = _poppingMessageFactory.Create(feedbackMessage);
                    feedback.AttachTo(_feedbackMessageAnchor);
                    
                    //Wait until end of message
                    var popOver = false;
                    feedback.PopOver += () => popOver = true;
                    await UniTask.WaitUntil(() => popOver);
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
            _movePlayerGameCameraTween?.Kill();
            
            var newPosition = new Vector3(
                _playerGameCamera.transform.position.x, 
                _playerCameraFocusPoint.transform.position.y + _gameData.cameraHeightOffsetFromTower, 
                _playerGameCamera.transform.position.z);
            
            _movePlayerGameCameraTween = _playerGameCamera.transform.DOMove(newPosition, _gameData.goToStepCameraMovementDuration);
            _movePlayerGameCameraTween.onComplete += () => _movePlayerGameCameraTween = null;
            _playerGameCamera.LookAt = _playerCameraFocusPoint.transform;
        }
        
        /// <summary>
        /// When a ball bonus is acquired
        /// </summary>
        /// <param name="value"></param>
        private void OnBallBonusAcquired(int value)
        {
            Debug.LogFormat("Ball bonus : +{0}", value);
        }
    }
}