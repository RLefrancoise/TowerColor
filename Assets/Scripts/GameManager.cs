using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Framework.Game;
using UniRx.Async;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace TowerColor
{
    /// <summary>
    /// The game manager
    /// </summary>
    public class GameManager : GameManagerBase
    {
        #region Fields

        /// <summary>
        /// Game data
        /// </summary>
        private GameData _gameData;
        
        /// <summary>
        /// Tower creator
        /// </summary>
        private TowerCreator _towerCreator;
        
        /// <summary>
        /// Ball bonus factory
        /// </summary>
        private BallBonus.Factory _ballBonusFactory;

        #endregion

        #region Properties

        /// <summary>
        /// The tower
        /// </summary>
        public Tower Tower { get; private set; }
        
        /// <summary>
        /// Ball bonuses
        /// </summary>
        public List<BallBonus> BallBonuses { get; private set; }

        #endregion

        #region Public Methods
        
        [Inject]
        public void Construct(GameData gameData, TowerCreator towerCreator, BallBonus.Factory ballBonusFactory)
        {
            _gameData = gameData;
            _towerCreator = towerCreator;
            _ballBonusFactory = ballBonusFactory;
            
            BallBonuses = new List<BallBonus>();
        }
        
        #endregion

        #region MonoBehaviours
        
        protected override async void Start()
        {
            DOTween.SetTweensCapacity(500,50);
            
            //Create tower
            _towerCreator.profile = _gameData.towerProfiles[Random.Range(0, _gameData.towerProfiles.Count)];
            _towerCreator.towerSteps = (int) LevelManager.GetCurveValue(_gameData.towerStepsByLevel);
            Tower = _towerCreator.GenerateTower();

            await UniTask.Yield();
            Tower.EnablePhysics(false);
            await Tower.ShuffleColors();

            //Place bonuses
            if (_gameData.useBallBonus)
            {
                var ballBonusesNumber = (int) LevelManager.GetCurveValue(_gameData.ballBonusesCount);
                var ballBonusStep = (float) (ballBonusesNumber + 2) / Tower.Steps.Count;

                for (var i = 1; i <= ballBonusesNumber; i++)
                {
                    var bonusSpawnData = _gameData.ballBonusesSpawnData[Random.Range(0, _gameData.ballBonusesSpawnData.Count)];
                    
                    var bonus = _ballBonusFactory.Create();
                    bonus.Value = bonusSpawnData.value;
                    bonus.RotateSpeed = Random.Range(bonusSpawnData.speedRange.x, bonusSpawnData.speedRange.y);

                    bonus.transform.position = Vector3.Lerp(
                        Tower.Steps[0].transform.position,
                        Tower.Steps.Last().transform.position, 
                        ballBonusStep * i);
                    bonus.transform.Translate(Tower.transform.forward * bonusSpawnData.distance, Space.World);
                    bonus.transform.RotateAround(Tower.transform.position, Tower.transform.up, Random.Range(0f, 360f));
                    bonus.transform.SetParent(Tower.transform, true);
                    
                    BallBonuses.Add(bonus);
                }
            }
            
            base.Start();
        }
        
        #endregion
    }
}