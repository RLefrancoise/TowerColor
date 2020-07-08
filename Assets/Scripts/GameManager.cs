using UnityEngine;
using Zenject;

namespace TowerColor
{
    /// <summary>
    /// The game manager handles the logic of the game
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// The tower creator to use to generate the tower
        /// </summary>
        private TowerCreator _towerCreator;

        private Tower _tower;
        
        [Inject]
        public void Construct(TowerCreator towerCreator)
        {
            _towerCreator = towerCreator;
        }

        private void Start()
        {
            _tower = _towerCreator.GenerateTower(0);
            _tower.EnablePhysics(true);
        }
    }
}