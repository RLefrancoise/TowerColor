using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace TowerColor
{
    [CreateAssetMenu(fileName = "GameData", menuName = "TowerColor/GameData")]
    public class GameData : ScriptableObject
    {
        /// <summary>
        /// Available brick colors
        /// </summary>
        [BoxGroup("Brick")]
        [Tooltip("Available brick colors")]
        public List<Material> brickColors;

        /// <summary>
        /// Material to use when a brick is inactive
        /// </summary>
        [BoxGroup("Brick")]
        [Tooltip("Material to use when a brick is inactive")]
        public Material inactiveBrickColor;

        /// <summary>
        /// Tower prefab
        /// </summary>
        [BoxGroup("Tower")]
        public GameObject towerPrefab;
        
        /// <summary>
        /// Available tower profiles
        /// </summary>
        [BoxGroup("Tower")]
        [Tooltip("Available towers")]
        public List<GameObject> towers;

        /// <summary>
        /// Max number of active steps in the tower
        /// </summary>
        [BoxGroup("Tower")]
        [Tooltip("Max number of active steps in the tower")]
        public int maxActiveSteps = 10;

        /// <summary>
        /// Rotation speed of the tower when the player is dragging
        /// </summary>
        [BoxGroup("Tower")]
        [Tooltip("Rotation speed of the tower when the player is dragging")]
        public float towerRotateSpeed = 1f;

        /// <summary>
        /// Probability that adjacent brick has the same color according to the current level
        /// </summary>
        [BoxGroup("Difficulty Settings")]
        [Tooltip("Probability that adjacent brick has the same color according to the current level")]
        public AnimationCurve sameColorForAdjacentBrickProbabilityByLevel;
    }
}