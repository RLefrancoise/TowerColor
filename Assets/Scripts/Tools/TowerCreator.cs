using NaughtyAttributes;
using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// It creates a tower according to some parameters
    /// </summary>
    [ExecuteInEditMode]
    public class TowerCreator : MonoBehaviour
    {
        /// <summary>
        /// Game data
        /// </summary>
        [SerializeField] private GameData gameData;

        /// <summary>
        /// Tower profile to use to generate the tower
        /// </summary>
        public TowerProfile profile;

        /// <summary>
        /// Tower steps
        /// </summary>
        public int towerSteps = 20;

        [Button("Generate tower")]
        public Tower GenerateTower()
        {
            var tower = Instantiate(gameData.towerPrefab).GetComponent<Tower>();
            
            //For each step
            for (var s = 0; s < towerSteps; ++s)
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
            }

            return tower;
        }
    }
}