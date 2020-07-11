using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace TowerColor
{
    public class Tower : MonoBehaviour
    {
        #region Fields
        
        private GameData _gameData;
        [ShowNonSerializedField] private int _currentStep;
        
        [SerializeField] private List<TowerStep> steps;
        
        #endregion

        #region Properties
        
        public ReadOnlyCollection<TowerStep> Steps => steps.AsReadOnly();

        public Transform CurrentSubTowerFocusPoint => GetStepFocusPoint(_currentStep);

        /// <summary>
        /// List of available colors in the tower
        /// </summary>
        public IEnumerable<Color> AvailableColors
        {
            get
            {
                var bricks = Steps.SelectMany(s => s.Bricks).Where(b => b.IsStillInPlace);
                return bricks.Select(b => b.Color);
            }
        }
        
        #endregion

        #region Events
        
        public event Action<int> CurrentStepChanged;
        
        #endregion

        [Inject]
        public void Construct(GameData gameData)
        {
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

        public Transform GetStepFocusPoint(int step)
        {
            var focusedStep = step - _gameData.maxActiveSteps / 2 + 1;
            if (focusedStep < 5) focusedStep = 5; // To avoid being too low and having the ball in the water, etc...
            return steps[focusedStep >= 0 ? focusedStep : 0].transform;
        }
        
        public void AddStep(TowerStep step)
        {
            steps.Add(step);
        }

        public void EnablePhysics(bool enable)
        {
            foreach (var step in steps)
            {
                step.EnablePhysics(enable);
            }
        }

        public void SetCurrentStep(int step)
        {
            if(step < 0 || step >= steps.Count) throw new Exception($"Invalid step {step}");

            //Deactivate all steps
            /*foreach (var s in steps)
            {
                s.ActivateStep(false);
            }*/

            for (var i = 0; i <= step - _gameData.maxActiveSteps; ++i)
            {
                if (i < 0) break;
                steps[i].ActivateStep(false);
            }

            //Activate step and all steps below it until reached max active steps
            for (var i = 0; i < _gameData.maxActiveSteps; ++i)
            {
                if (step - i < 0) break;
                steps[step - i].ActivateStep(true);
            }

            _currentStep = step;
            CurrentStepChanged?.Invoke(step);
        }

        /// <summary>
        /// Init the tower for the current level
        /// </summary>
        /// <param name="level">Level number</param>
        public void Init(int level)
        {
            //Reset bricks color to first color
            foreach (var step in steps)
            {
                foreach (var brick in step.Bricks)
                    brick.Color = _gameData.brickColors[0].color;
            }

            //Color each brick according to its surrounding
            foreach (var step in steps)
            {
                foreach (var brick in step.Bricks)
                {
                    foreach (var b in brick.GetSurroundingBricks())
                    {
                        //According to the level, we have a specific change that surrounding bricks have same color
                        var randomChance = Random.Range(0f, 1f);
                        if (randomChance <= _gameData.sameColorForAdjacentBrickProbabilityByLevel.Evaluate((level - 1) / 100f))
                        {
                            b.Color = brick.Color;
                        }
                        else
                        {
                            //We take a random color from the color list, except the same color as the brick
                            b.Color = _gameData.brickColors.Select(x => x.color).Where(x => x != brick.Color)
                                .ElementAt(Random.Range(0, _gameData.brickColors.Count - 1));
                        }
                    }
                }
            }
        }

        public bool IsBrickTargetable(Brick brick)
        {
            if (brick.IsInWater) return false;
            if (brick.HasFellOnPlatform) return false;
            if (!brick.IsActivated) return false;
            if (brick.Velocity.sqrMagnitude >= _gameData.targetableBrickSquaredVelocityThreshold) return false;

            return true;
        }

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
        private void OnStepFullyDestroyed()
        {
            for (var i = steps.Count - 1; i >= 0; --i)
            {
                if (steps[i].IsFullyDestroyed) continue;
                
                SetCurrentStep(i);
                break;
            }
        }
        
        #endregion
    }
}