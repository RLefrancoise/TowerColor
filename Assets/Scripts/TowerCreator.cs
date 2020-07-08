using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

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

        public void GenerateTower(int level)
        {
            var profile = _gameData.towerProfiles[0];

            var brickHeight = -1f;
            var rotationStep = 360f / profile.bricksPerStep;

            //For each step
            for (var s = 0; s < profile.stepCount; ++s)
            {
                //Create step object to group bricks per step
                var stepObject = new GameObject($"Step_{s+1}");
                stepObject.transform.SetParent(transform, false);
                
                //Move step object to correct step height
                stepObject.transform.Translate(brickHeight * s * stepObject.transform.up);
                
                //Reset rotation and apply rotation offset
                stepObject.transform.localRotation = Quaternion.identity;
                stepObject.transform.RotateAround(stepObject.transform.position, stepObject.transform.up, profile.rotationAmountPerStep * s);
                
                //Create each brick for the current step
                for (var i = 0f; i <= 360f ; i += rotationStep)
                {
                    var x = stepObject.transform.position.x + Mathf.Cos(i * Mathf.Deg2Rad) * profile.towerRadius;
                    var z = stepObject.transform.position.z + Mathf.Sin(i * Mathf.Deg2Rad) * profile.towerRadius;
                    
                    var brickPosition = new Vector3(x, stepObject.transform.position.y, z);
                    var brickRotation = Quaternion.identity;
                    
                    var brick = Instantiate(profile.brickPrefab, brickPosition, brickRotation, stepObject.transform).GetComponent<Brick>();
                    
                    //If brick height not yet defined, define it
                    if (brickHeight == -1f) brickHeight = brick.Height;
                    
                    //Choose random color for now
                    brick.Color = _gameData.brickColors.Select(c => c.color)
                        .ElementAt(Random.Range(0, _gameData.brickColors.Count));
                }
            }
        }
    }
}