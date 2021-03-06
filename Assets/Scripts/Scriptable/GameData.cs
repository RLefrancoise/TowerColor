using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// The game data
    /// </summary>
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

        /// <summary>
        /// Brick destroy sound
        /// </summary>
        [BoxGroup("Brick")]
        [Tooltip("Brick destroy sound")]
        public AudioClip brickDestroySound;

        public enum AdjacentBrickFindMode
        {
            RayCast,
            SphereCast,
            Trigger
        }
        
        /// <summary>
        /// Use sphere cast or ray cast to find adjacent bricks.
        /// Sphere cast is faster but it may miss adjacent bricks that are not close enough
        /// Ray cast is dependant of rotation so it may miss some adjacent bricks sometimes
        /// Trigger should be the preferred way
        /// </summary>
        [BoxGroup("Brick")]
        [Tooltip("Use sphere cast or ray cast to find adjacent bricks.\n" +
                 "Sphere cast is faster but it may miss adjacent bricks that are not close enough\n" +
                 "Ray cast is dependant of rotation so it may miss some adjacent bricks sometimes\n" +
                 "Trigger should be the preferred way")]
        public AdjacentBrickFindMode brickUseSphereCastToFindAdjacentBricks = AdjacentBrickFindMode.Trigger;
        
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
        /// Tower profiles
        /// </summary>
        [BoxGroup("Tower")]
        [Tooltip("Tower profiles")]
        public List<TowerProfile> towerProfiles;

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
        
        /// <summary>
        /// Ball prefab
        /// </summary>
        [BoxGroup("Ball")]
        [Tooltip("Ball prefab")]
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

        /// <summary>
        /// Height offset of camera
        /// </summary>
        [BoxGroup("Game Camera")]
        [Tooltip("Height offset of camera")]
        public float cameraHeightOffsetFromTower = 2f;

        /// <summary>
        /// Should the focus of the camera being centered on the current tower steps ?
        /// </summary>
        [BoxGroup("Game Camera")]
        [Tooltip("Should the focus of the camera being centered on the current tower steps ?")]
        public bool cameraFocusCenteredOnSteps = true;

        /// <summary>
        /// The distance the camera is going when winning
        /// </summary>
        [BoxGroup("Game Camera")]
        [Tooltip("The distance the camera is going when winning")]
        public float cameraDistanceOnWin = 50f;

        /// <summary>
        /// Duration of the camera movement when winning
        /// </summary>
        [BoxGroup("Game Camera")]
        [Tooltip("Duration of the camera movement when winning")]
        public float cameraMoveDurationOnWin = 2f;
        
        #endregion

        /// <summary>
        /// Force that is applied to the bricks that fell into water
        /// </summary>
        [BoxGroup("Water Ground")]
        [Tooltip("Force that is applied to the bricks that fell into water")]
        public float waterGroundForce = 20f;

        /// <summary>
        /// Water ground max brick speed
        /// </summary>
        [BoxGroup("Water Ground")]
        [Tooltip("Water ground max brick speed")]
        public float waterGroundMaxBrickSpeed = 5f;

        /// <summary>
        /// Max distance the bricks are drifting away from the tower
        /// </summary>
        [BoxGroup("Water Ground")]
        [Tooltip("Max distance the bricks are drifting away from the tower")]
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
        /// Number of tower steps for the current level
        /// </summary>
        [BoxGroup("Difficulty Settings")]
        [Tooltip("Number of tower steps for the current level")]
        public AnimationCurve towerStepsByLevel;

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

        /// <summary>
        /// Good message prefab
        /// </summary>
        [BoxGroup("Popping Messages")]
        [Tooltip("Good message prefab")]
        public GameObject goodMessage;
        
        /// <summary>
        /// Great message prefab
        /// </summary>
        [BoxGroup("Popping Messages")]
        [Tooltip("Great message prefab")]
        public GameObject greatMessage;
        
        /// <summary>
        /// Insane message prefab
        /// </summary>
        [BoxGroup("Popping Messages")]
        [Tooltip("Insane message prefab")]
        public GameObject insaneMessage;

        /// <summary>
        /// Ball gained message prefab
        /// </summary>
        [BoxGroup("Popping Messages")]
        [Tooltip("Ball gained message prefab")]
        public GameObject ballGainedMessage;

        /// <summary>
        /// Bricks to break to display good message
        /// </summary>
        [BoxGroup("Popping Messages")]
        [Tooltip("Bricks to break to display good message")]
        public int goodMessageBricksCount = 4;
        
        /// <summary>
        /// Bricks to break to display great message
        /// </summary>
        [BoxGroup("Popping Messages")]
        [Tooltip("Bricks to break to display great message")]
        public int greatMessageBricksCount = 8;
        
        /// <summary>
        /// Bricks to break to display insane message
        /// </summary>
        [BoxGroup("Popping Messages")]
        [Tooltip("Bricks to break to display insane message")]
        public int insaneMessageBricksCount = 15;

        #region Bonuses

        /// <summary>
        /// Use ball bonus ?
        /// </summary>
        [BoxGroup("Bonuses")]
        [Tooltip("Use ball bonus ?")]
        public bool useBallBonus = false;
        
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
            public Vector2 speedRange;
        }

        /// <summary>
        /// Ball bonuses spawn data
        /// </summary>
        [BoxGroup("Bonuses")]
        [Tooltip("Ball bonuses spawn data")]
        public List<BallBonusSpawnData> ballBonusesSpawnData;

        /// <summary>
        /// Balls gained when a good message appears
        /// </summary>
        [BoxGroup("Bonuses")]
        [Tooltip("Balls gained when a good message appears")]
        public int ballsGainedAfterGoodMessage = 0;
        
        /// <summary>
        /// Balls gained when a great message appears
        /// </summary>
        [BoxGroup("Bonuses")]
        [Tooltip("Balls gained when a great message appears")]
        public int ballsGainedAfterGreatMessage = 1;
        
        /// <summary>
        /// Balls gained when an insane message appears
        /// </summary>
        [BoxGroup("Bonuses")]
        [Tooltip("Balls gained when an insane message appears")]
        public int ballsGainedAfterInsaneMessage = 2;

        #endregion

        /// <summary>
        /// Win particle effect
        /// </summary>
        [BoxGroup("Win")]
        [Tooltip("Win particle effect")]
        public GameObject winEffect;

        /// <summary>
        /// Sequence of win particle effect scaling
        /// </summary>
        [BoxGroup("Win")]
        [Tooltip("Sequence of win particle effect scaling")]
        public List<float> winEffectScalingSequence;

        /// <summary>
        /// Time between each win effect
        /// </summary>
        [BoxGroup("Win")]
        [Tooltip("Time between each win effect")]
        public float winEffectTimeBetweenEach = 0.5f;

        
        /// <summary>
        /// Fever gauge max capacity
        /// </summary>
        [BoxGroup("Fever")]
        [Tooltip("Fever gauge max capacity")]
        public int feverGaugeMaxCapacity = 50;

        /// <summary>
        /// Fever gain by destroyed brick
        /// </summary>
        [BoxGroup("Fever")]
        [Tooltip("Fever gain by destroyed brick")]
        public int feverGainByBrick = 1;
        
        /// <summary>
        /// Fever points gained when a good message appears
        /// </summary>
        [BoxGroup("Fever")]
        [Tooltip("Fever points gained when a good message appears")]
        public int feverGaugeGainWithGood = 2;
        
        /// <summary>
        /// [Tooltip("Fever points gained when a great message appears")]
        /// </summary>
        [BoxGroup("Fever")]
        [Tooltip("Fever points gained when a great message appears")]
        public int feverGaugeGainWithGreat = 5;
        
        /// <summary>
        /// Fever points gained when an insane message appears
        /// </summary>
        [BoxGroup("Fever")]
        [Tooltip("Fever points gained when an insane message appears")]
        public int feverGaugeGainWithInsane = 10;

        /// <summary>
        /// Fever message prefab
        /// </summary>
        [BoxGroup("Fever")]
        [Tooltip("Fever message prefab")]
        public GameObject feverMessage;

        /// <summary>
        /// Number of rainbow balls per fever
        /// </summary>
        [BoxGroup("Fever")]
        [Tooltip("Number of rainbow balls per fever")]
        public int feverRainbowBallsCount = 3;
    }
}