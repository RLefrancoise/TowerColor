using UnityEngine;
using Zenject;

namespace TowerColor
{
    /// <summary>
    /// Tower spawner (not used anymore, replaced by TowerCreator)
    /// </summary>
    public class TowerSpawner : MonoBehaviour
    {
        /// <summary>
        /// Game data
        /// </summary>
        private GameData _gameData;
        
        [Inject]
        public void Construct(GameData gameData)
        {
            _gameData = gameData;
        }

        /// <summary>
        /// Spawn random tower
        /// </summary>
        /// <param name="level">For level</param>
        /// <returns></returns>
        public Tower SpawnRandomTower(int level)
        {
            return Instantiate(_gameData.towers[Random.Range(0, _gameData.towers.Count)]).GetComponent<Tower>();
        }
    }
}