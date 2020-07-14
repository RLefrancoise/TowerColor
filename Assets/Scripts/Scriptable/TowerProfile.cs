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
        public GameObject stepPrefab;

        /// <summary>
        /// Rotation amount of a tower step according to the previous one
        /// </summary>
        public float rotationAmountPerStep = 5f;
    }
}