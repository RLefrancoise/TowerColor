using Framework.Game;
using Zenject;

namespace TowerColor
{
    /// <summary>
    /// The game manager handles the logic of the game
    /// </summary>
    public class GameManager : GameManagerBase
    {
        #region Fields

        private TowerSpawner _towerSpawner;

        #endregion

        #region Properties

        public Tower Tower { get; private set; }

        #endregion

        #region Public Methods
        
        [Inject]
        public void Construct(TowerSpawner towerSpawner)
        {
            _towerSpawner = towerSpawner;
        }
        
        #endregion

        #region MonoBehaviours
        
        protected override void Start()
        {
            base.Start();

            //Create tower
            Tower = _towerSpawner.SpawnRandomTower(LevelManager.CurrentLevel);
            Tower.ShuffleColors();
            Tower.EnablePhysics(false);
        }
        
        #endregion
    }
}