using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// Tower profile. Used to generate a tower
    /// </summary>
    [CreateAssetMenu(fileName = "Tower Profile", menuName = "TowerColor/Tower Profile")]
    public class TowerProfile : ScriptableObject
    {
        /// <summary>
        /// Brick to use to generate the tower
        /// </summary>
        public GameObject brickPrefab;

        /// <summary>
        /// The number of steps
        /// </summary>
        public int stepCount = 10;
        
        /// <summary>
        /// Radius of the tower
        /// </summary>
        public float towerRadius = 1f;

        /// <summary>
        /// Rotation step per brick on a given step
        /// </summary>
        public int bricksPerStep = 10;
        
        /// <summary>
        /// Rotation amount of a tower step according to the previous one
        /// </summary>
        public float rotationAmountPerStep = 5f;
    }
}