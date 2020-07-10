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
        /// Squared velocity threshold to consider a brick targetable
        /// </summary>
        [BoxGroup("Brick")]
        [Tooltip("Squared velocity threshold to consider a brick targetable")]
        public float targetableBrickSquaredVelocityThreshold = 0.04f;
        
        #region Tower
        
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
        
        #endregion

        #region Ball
        
        [BoxGroup("Ball")]
        public GameObject ballPrefab;

        /// <summary>
        /// Fire duration of the ball
        /// </summary>
        [BoxGroup("Ball")]
        [Tooltip("Fire duration of the ball")]
        public float ballFireDuration = 1f;

        /// <summary>
        /// Ball distance from camera
        /// </summary>
        [BoxGroup("Ball")]
        [Tooltip("Ball distance from camera")]
        public float ballDistanceFromCamera = 10f;
        
        #endregion

        #region Cameras
        
        /// <summary>
        /// Duration of the camera movement when current tower step is changed
        /// </summary>
        [BoxGroup("Game Camera")]
        [Tooltip("Duration of the camera movement when current tower step is changed")]
        public float goToStepCameraMovementDuration = 0.5f;
        
        #endregion

        /// <summary>
        /// Force that is applied to the bricks that fell into water
        /// </summary>
        [BoxGroup("Water Ground")]
        [Tooltip("Force that is applied to the bricks that fell into water")]
        public float waterGroundForce = 20f;

        [BoxGroup("Water Ground")]
        public float waterGroundMaxBrickSpeed = 5f;

        [BoxGroup("Water Ground")]
        public float waterGroundMaxBrickDistance = 25f;
        
        #region Difficulty Settings
        
        /// <summary>
        /// Probability that adjacent brick has the same color according to the current level
        /// </summary>
        [BoxGroup("Difficulty Settings")]
        [Tooltip("Probability that adjacent brick has the same color according to the current level")]
        public AnimationCurve sameColorForAdjacentBrickProbabilityByLevel;

        /// <summary>
        /// The number of balls by level
        /// </summary>
        [BoxGroup("Difficulty Settings")]
        [Tooltip("The number of balls by level")]
        public AnimationCurve numberOfBallsByLevel;
        
        #endregion
    }
}