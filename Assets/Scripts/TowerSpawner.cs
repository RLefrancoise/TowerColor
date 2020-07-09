using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class TowerSpawner : MonoBehaviour
    {
        private GameData _gameData;
        
        [Inject]
        public void Construct(GameData gameData)
        {
            _gameData = gameData;
        }

        public Tower SpawnRandomTower(int level)
        {
            return Instantiate(_gameData.towers[Random.Range(0, _gameData.towers.Count)]).GetComponent<Tower>();
        }
    }
}