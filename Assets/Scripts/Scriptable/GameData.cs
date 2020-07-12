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

        /// <summary>
        /// Brick effect when destroyed by a ball
        /// </summary>
        [BoxGroup("Brick")]
        [Tooltip("Brick effect when destroyed by a ball")]
        public GameObject brickDestroyEffect;
        
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

        /// <summary>
        /// The minimum destroy ratio of a step to be considered destroyed
        /// </summary>
        [BoxGroup("Tower")]
        [Tooltip("The minimum destroy ratio of a step to be considered destroyed")]
        public float towerStepDestroyedMinimumRatio = 1f;

        /// <summary>
        /// Color change transition duration
        /// </summary>
        [BoxGroup("Tower")]
        [Tooltip("Color change transition duration")]
        public float colorChangeDuration = 1f;
        
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
        /// Force of the ball jump
        /// </summary>
        [BoxGroup("Ball")]
        [Tooltip("Force of the ball jump")]
        public float ballJumpForce = 1f;

        /// <summary>
        /// Number of turns the ball is doing on itself when being fired
        /// </summary>
        [BoxGroup("Ball")]
        [Tooltip("Number of turns the ball is doing on itself when being fired")]
        public float ballTurnNumber = 5f;

        /// <summary>
        /// Repulse distance of the ball if it hits a brick with a different color
        /// </summary>
        [BoxGroup("Ball")]
        [Tooltip("Repulse distance of the ball if it hits a brick with a different color")]
        public float ballRepulseDistance = 5f;

        /// <summary>
        /// Duration of the ball repulsion before it is destroyed
        /// </summary>
        [BoxGroup("Ball")]
        [Tooltip("Duration of the ball repulsion before it is destroyed")]
        public float ballRepulseDuration = 0.5f;

        /// <summary>
        /// Position of the ball on screen
        /// </summary>
        [BoxGroup("Ball")]
        [Tooltip("Position of the ball on screen")]
        public Vector2 ballPositionOnScreen = new Vector2(0.5f, 0.2f);
        
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

        /// <summary>
        /// Distance of the camera from the tower
        /// </summary>
        [BoxGroup("Game Camera")]
        [Tooltip("Distance of the camera from the tower")]
        public float cameraDistanceFromTower = 15f;
        
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

        /// <summary>
        /// The number of balls to fire to trigger color change
        /// </summary>
        [BoxGroup("Difficulty Settings")]
        [Tooltip("The number of balls to fire to trigger color change")]
        public AnimationCurve ballsToFireToTriggerColorChange;

        /// <summary>
        /// The minimum step the tower must be currently to win
        /// </summary>
        [BoxGroup("Difficulty Settings")]
        [Tooltip("The minimum step the tower must be currently to win")]
        public int minimumTowerStepToWin = 3;

        /// <summary>
        /// The minimum ratio of destroyed bricks in the last step to be able to win
        /// </summary>
        [BoxGroup("Difficulty Settings")]
        [Tooltip("The minimum ratio of destroyed bricks in the last step to be able to win")]
        [Range(0f, 1f)]
        public float destroyRatioOfBottomTowerToWin = 0.5f;

        #endregion
        
        /// <summary>
        /// Color change message prefab
        /// </summary>
        [BoxGroup("Popping Messages")]
        [Tooltip("Color change message prefab")]
        public GameObject colorChangeMessage;

        #region Bonuses
        
        /// <summary>
        /// Ball bonus
        /// </summary>
        [BoxGroup("Bonuses")]
        [Tooltip("Ball bonus")]
        public GameObject ballBonus;

        /// <summary>
        /// Number of ball bonuses by level
        /// </summary>
        [BoxGroup("Bonuses")]
        [Tooltip("Number of ball bonuses by level")]
        public AnimationCurve ballBonusesCount;

        [Serializable]
        public class BallBonusSpawnData
        {
            public int value;
            public float distance;
        }

        /// <summary>
        /// Ball bonuses spawn data
        /// </summary>
        [BoxGroup("Bonuses")]
        [Tooltip("Ball bonuses spawn data")]
        public List<BallBonusSpawnData> ballBonusesSpawnData;

        #endregion
    }
}