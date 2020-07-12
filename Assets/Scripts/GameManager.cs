using System.Linq;
using Framework.Game;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    /// <summary>
    /// The game manager handles the logic of the game
    /// </summary>
    public class GameManager : GameManagerBase
    {
        #region Fields

        private GameData _gameData;
        private TowerSpawner _towerSpawner;
        private BallBonus.Factory _ballBonusFactory;

        #endregion

        #region Properties

        public Tower Tower { get; private set; }

        #endregion

        #region Public Methods
        
        [Inject]
        public void Construct(GameData gameData, TowerSpawner towerSpawner, BallBonus.Factory ballBonusFactory)
        {
            _gameData = gameData;
            _towerSpawner = towerSpawner;
            _ballBonusFactory = ballBonusFactory;
        }
        
        #endregion

        #region MonoBehaviours
        
        protected override async void Start()
        {
            base.Start();

            //Create tower
            Tower = _towerSpawner.SpawnRandomTower(LevelManager.CurrentLevel);
            Tower.EnablePhysics(false);
            await Tower.ShuffleColors();
            
            //Place bonuses
            var ballBonusesNumber = (int) LevelManager.GetCurveValue(_gameData.ballBonusesCount);
            var ballBonusStep = (float) (ballBonusesNumber - 2) / Tower.Steps.Count;

            for (var i = 1; i <= ballBonusesNumber; i++)
            {
                var bonusSpawnData = _gameData.ballBonusesSpawnData[Random.Range(0, _gameData.ballBonusesSpawnData.Count)];
                
                var bonus = _ballBonusFactory.Create();
                bonus.Value = bonusSpawnData.value;

                bonus.transform.position = Vector3.Lerp(
                    Tower.Steps[0].transform.position,
                    Tower.Steps.Last().transform.position, 
                    ballBonusStep * i);
                bonus.transform.Translate(Tower.transform.forward * bonusSpawnData.distance, Space.World);
                bonus.transform.RotateAround(Tower.transform.position, Tower.transform.up, Random.Range(0f, 360f));
            }
        }
        
        #endregion
    }
}