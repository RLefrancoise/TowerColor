using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace TowerColor
{
    /// <summary>
    /// It creates a tower according to some parameters
    /// </summary>
    public class TowerCreator : MonoBehaviour
    {
        /// <summary>
        /// Game data
        /// </summary>
        private GameData _gameData;
        
        [Inject]
        public void Construct(GameData gameData)
        {
            _gameData = gameData;
        }

        public Tower GenerateTower(int level)
        {
            var towerObject = new GameObject("Tower");
            var tower = towerObject.AddComponent<Tower>();

            var profile = _gameData.towerProfiles[0];
            
            //For each step
            for (var s = 0; s < profile.stepCount; ++s)
            {
                var stepPosition = s == 0
                    ? tower.transform.position
                    : tower.Steps[s - 1].transform.position + tower.transform.up * tower.Steps[s - 1].Height;

                var stepRotation = s == 0
                    ? tower.transform.rotation
                    : tower.Steps[s - 1].transform.rotation * Quaternion.Euler(0f, profile.rotationAmountPerStep, 0f);
                
                //Create step object to group bricks per step
                var step = Instantiate(profile.stepPrefab, stepPosition, stepRotation, tower.transform).GetComponent<TowerStep>();
                step.name = $"Step_{s + 1}";

                //Add step to tower
                tower.AddStep(step);
                
                //Color each brick according to its surrounding
                foreach (var brick in step.Bricks)
                    brick.Color = _gameData.brickColors[0].color;
                
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

            return tower;
        }
    }
}