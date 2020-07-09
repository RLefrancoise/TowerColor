using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace TowerColor
{
    public class Tower : MonoBehaviour
    {
        private GameData _gameData;
        
        [SerializeField] private List<TowerStep> steps;

        public ReadOnlyCollection<TowerStep> Steps => steps.AsReadOnly();

        public Transform CurrentSubTowerFocusPoint
        {
            get
            {
                var activeSteps = steps.Where(s => s.IsActivated).ToList();
                return activeSteps.Count > 0 ? activeSteps[activeSteps.Count / 2].transform : null;
            }
        }

        public event Action<int> CurrentStepChanged;

        [Inject]
        public void Construct(GameData gameData)
        {
            _gameData = gameData;
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
            foreach (var s in steps)
            {
                s.ActivateStep(false);
            }

            //Activate step and all steps below it until reached max active steps
            for (var i = 0; i < _gameData.maxActiveSteps; ++i)
            {
                if (step - i < 0) break;
                steps[step - i].ActivateStep(true);
            }
            
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
                    var hits = new RaycastHit[10];
                    var size = Physics.BoxCastNonAlloc(brick.Center, brick.Bounds.extents, brick.transform.up, hits, brick.transform.rotation);
                    
                    for (var i = 0; i < size; ++i)
                    {
                        var hitBrick = hits[i].collider.GetComponentInParent<Brick>();
                        if (!hitBrick) continue;

                        var randomChance = Random.Range(0f, 1f);
                        if (randomChance <= _gameData.sameColorForAdjacentBrickProbabilityByLevel.Evaluate((level - 1) / 100f))
                        {
                            hitBrick.Color = brick.Color;
                        }
                        else
                        {
                            hitBrick.Color = _gameData.brickColors.Select(x => x.color).Where(x => x != brick.Color)
                                .ElementAt(Random.Range(0, _gameData.brickColors.Count - 1));
                        }
                    }
                }
            }
        }
    }
}