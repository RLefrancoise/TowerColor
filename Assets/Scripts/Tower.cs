using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NaughtyAttributes;
using UniRx.Async;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace TowerColor
{
    /// <summary>
    /// The tower color tower
    /// </summary>
    public class Tower : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Game manager
        /// </summary>
        private GameManager _gameManager;
        
        /// <summary>
        /// Game data
        /// </summary>
        private GameData _gameData;
        
        /// <summary>
        /// Tower steps
        /// </summary>
        [SerializeField] private List<TowerStep> steps;
        
        #endregion

        #region Properties
        
        /// <summary>
        /// Current step
        /// </summary>
        [ShowNativeProperty] public int CurrentStep { get; private set; }

        /// <summary>
        /// Tower steps
        /// </summary>
        public ReadOnlyCollection<TowerStep> Steps => steps.AsReadOnly();

        /// <summary>
        /// Current sub tower focus point
        /// </summary>
        public Transform CurrentSubTowerFocusPoint => GetStepFocusPoint(CurrentStep);

        /// <summary>
        /// List of available colors in the tower
        /// </summary>
        public IEnumerable<Color> AvailableColors
        {
            get
            {
                var bricks = Steps.SelectMany(s => s.Bricks).Where(b => b.IsActivated && b.IsStillInPlace);
                return bricks.Select(b => b.Color);
            }
        }
        
        #endregion

        #region Events
        
        /// <summary>
        /// When current step is changed
        /// </summary>
        public event Action<int> CurrentStepChanged;
        
        #endregion

        [Inject]
        public void Construct(GameManager gameManager, GameData gameData)
        {
            _gameManager = gameManager;
            _gameData = gameData;
        }

        #region MonoBehaviours

        private void Start()
        {
            foreach (var step in steps)
            {
                step.FullyDestroyed += OnStepFullyDestroyed;
            }
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Get step focus point
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public Transform GetStepFocusPoint(int step)
        {
            if (_gameData.cameraFocusCenteredOnSteps)
            {
                var focusedStep = step - _gameData.maxActiveSteps / 2 + 1;
                if (focusedStep < 5) focusedStep = 5; // To avoid being too low and having the ball in the water, etc...
                return steps[focusedStep >= 0 ? focusedStep : 0].transform;
            }
            else
            {
                var focusedStep = step - _gameData.maxActiveSteps;
                if (focusedStep < 0) focusedStep = 0;
                return steps[focusedStep].transform;
            }
        }
        
        /// <summary>
        /// Add a step to the tower
        /// </summary>
        /// <param name="step"></param>
        public void AddStep(TowerStep step)
        {
            steps.Add(step);
        }

        /// <summary>
        /// Initialize tower state (current start position of bricks, ...)
        /// </summary>
        public void InitializeState()
        {
            foreach (var step in steps)
            {
                step.InitializeState();
            }
        }
        
        /// <summary>
        /// Enable tower physics
        /// </summary>
        /// <param name="enable">Enable or disable</param>
        public void EnablePhysics(bool enable)
        {
            foreach (var step in steps)
            {
                if(step.IsActivated)
                    step.EnablePhysics(enable);
            }
        }

        /// <summary>
        /// Set current step
        /// </summary>
        /// <param name="step">Current step</param>
        /// <param name="waitBetweenSteps">Wait between each step</param>
        /// <param name="force">Force step activation</param>
        public async UniTask SetCurrentStep(int step, bool waitBetweenSteps = true, bool force = false)
        {
            if(step < 0 || step >= steps.Count) throw new Exception($"Invalid step {step}");

            //Deactivate all steps
            for (var i = 0; i <= step - _gameData.maxActiveSteps; ++i)
            {
                if (i < 0) break;
                if (force || steps[i].IsActivated)
                {
                    steps[i].ActivateStep(false, force);
                    if(waitBetweenSteps) await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
                }
            }
            
            //Activate step and all steps below it until reached max active steps
            for (var i = 0; i < _gameData.maxActiveSteps; ++i)
            {
                if (step - i < 0) break;
                if (force || !steps[step - i].IsActivated)
                {
                    steps[step - i].ActivateStep(true, force);
                    if(waitBetweenSteps) await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
                }
            }

            CurrentStep = step;
            CurrentStepChanged?.Invoke(step);
        }

        public async UniTask ShuffleColors(GameData.AdjacentBrickFindMode mode, bool ignoreInactiveSteps = false, bool lerp = false,
            bool randomBeforeShuffle = true)
        {
            Debug.Log("Shuffle colors");
            
            //Reset bricks color to first color
            if (randomBeforeShuffle)
            {
                foreach (var step in steps)
                {
                    //If step is inactive, ignore it
                    if (ignoreInactiveSteps && !step.IsActivated) continue;
                    
                    foreach (var brick in step.Bricks)
                        brick.Color = _gameData.brickColors[Random.Range(0, _gameData.brickColors.Count)].color;
                }
            }

            var bricksAlreadyChanged = new List<Brick>();
            
            //Color each brick according to its surrounding
            for(var i = steps.Count - 1 ; i >= 0 ; i--)
            {
                var step = steps[i];
                
                //If step is inactive, ignore it
                //if (ignoreInactiveSteps && !step.IsActivated) break;
                if (ignoreInactiveSteps && i < (CurrentStep - _gameData.maxActiveSteps)) break; // Steps below should not be activated anyway
                
                foreach (var brick in step.Bricks)
                {
                    foreach (var b in brick.GetSurroundingBricks(mode, !ignoreInactiveSteps))
                    {
                        //Don't take non activated bricks, otherwise, it ends with invalid state between color and activation status...
                        if (!b.IsActivated) continue;
                        
                        //Dont shuffle twice the same brick
                        if (bricksAlreadyChanged.Contains(b)) continue;
                        
                        //According to the level, we have a specific change that surrounding bricks have same color
                        var randomChance = Random.Range(0f, 1f);
                        if (randomChance <= _gameManager.LevelManager.GetCurveValue(_gameData.sameColorForAdjacentBrickProbabilityByLevel))
                        {
                            if(lerp) b.LerpColor(brick.Color, _gameData.colorChangeDuration);
                            else b.Color = brick.Color;
                        }
                        else
                        {
                            //We take a random color from the color list, except the same color as the brick
                            var c = _gameData.brickColors.Select(x => x.color).Where(x => x != brick.Color)
                                .ElementAt(Random.Range(0, _gameData.brickColors.Count - 1));
                            
                            if(lerp) b.LerpColor(c, _gameData.colorChangeDuration);
                            b.Color = c;
                        }
                        
                        bricksAlreadyChanged.Add(b);
                    }
                }
            }

            if (lerp) await UniTask.Delay(TimeSpan.FromSeconds(_gameData.colorChangeDuration));
        }
        
        /// <summary>
        /// Shuffle the tower colors
        /// </summary>
        public async UniTask ShuffleColors(bool ignoreInactiveSteps = false, bool lerp = false, bool randomBeforeShuffle = true)
        {
            await ShuffleColors(_gameData.brickUseSphereCastToFindAdjacentBricks, ignoreInactiveSteps, lerp,
                randomBeforeShuffle);
            
            /*Debug.Log("Shuffle colors");
            
            //Reset bricks color to first color
            if (resetBeforeShuffle)
            {
                foreach (var step in steps)
                {
                    //If step is inactive, ignore it
                    if (ignoreInactiveSteps && !step.IsActivated) continue;
                    
                    foreach (var brick in step.Bricks)
                        brick.Color = _gameData.brickColors[0].color;
                }
            }

            var bricksAlreadyChanged = new List<Brick>();
            
            //Color each brick according to its surrounding
            for(var i = steps.Count - 1 ; i >= 0 ; i--)
            {
                var step = steps[i];
                
                //If step is inactive, ignore it
                //if (ignoreInactiveSteps && !step.IsActivated) break;
                if (ignoreInactiveSteps && i < (CurrentStep - _gameData.maxActiveSteps)) break; // Steps below should not be activated anyway
                
                foreach (var brick in step.Bricks)
                {
                    foreach (var b in brick.GetSurroundingBricks(!ignoreInactiveSteps))
                    {
                        //Don't take non activated bricks, otherwise, it ends with invalid state between color and activation status...
                        if (!b.IsActivated) continue;
                        
                        //Dont shuffle twice the same brick
                        if (bricksAlreadyChanged.Contains(b)) continue;
                        
                        //According to the level, we have a specific change that surrounding bricks have same color
                        var randomChance = Random.Range(0f, 1f);
                        if (randomChance <= _gameManager.LevelManager.GetCurveValue(_gameData.sameColorForAdjacentBrickProbabilityByLevel))
                        {
                            if(lerp) b.LerpColor(brick.Color, _gameData.colorChangeDuration);
                            else b.Color = brick.Color;
                        }
                        else
                        {
                            //We take a random color from the color list, except the same color as the brick
                            var c = _gameData.brickColors.Select(x => x.color).Where(x => x != brick.Color)
                                .ElementAt(Random.Range(0, _gameData.brickColors.Count - 1));
                            
                            if(lerp) b.LerpColor(c, _gameData.colorChangeDuration);
                            b.Color = c;
                        }
                        
                        bricksAlreadyChanged.Add(b);
                    }
                }
            }

            if (lerp) await UniTask.Delay(TimeSpan.FromSeconds(_gameData.colorChangeDuration));*/
        }

        /// <summary>
        /// Is brick targetable ?
        /// </summary>
        /// <param name="brick">Brick</param>
        /// <returns></returns>
        public bool IsBrickTargetable(Brick brick)
        {
            if (brick.IsInWater) return false;
            //if (brick.HasFellOnPlatform) return false;
            if (!brick.IsActivated) return false;
            if (brick.Velocity.sqrMagnitude >= _gameData.targetableBrickSquaredVelocityThreshold) return false;

            return true;
        }

        /// <summary>
        /// Get bricks with the same color as the given brick
        /// </summary>
        /// <param name="brick">Brick</param>
        /// <returns></returns>
        public List<Brick> GetBricksWithSameColor(Brick brick)
        {
            var bricks = new List<Brick> {brick};
            
            RecurseOnBricksWithSameColor(bricks, brick);
            
            return bricks;
        }
        
        #endregion

        #region Private Methods

        private void RecurseOnBricksWithSameColor(List<Brick> withSameColor, Brick current)
        {
            foreach (var brick in current.GetSurroundingBricks(false))
            {
                if (withSameColor.Contains(brick)) continue;
                
                if (!brick.IsActivated) continue;
                //if (!brick.IsStillInPlace) continue;
                if (brick.Color != current.Color) continue;
                
                withSameColor.Add(brick);

                RecurseOnBricksWithSameColor(withSameColor, brick);
            }
        }
        
        /// <summary>
        /// When step is fully destroyed, update new current step
        /// </summary>
        private async void OnStepFullyDestroyed()
        {
            for (var i = steps.Count - 1; i >= 0; --i)
            {
                if (steps[i].IsFullyDestroyed) continue;
                
                await SetCurrentStep(i);
                break;
            }
        }
        
        #endregion
    }
}