using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class BallSpawner : MonoBehaviour
    {
        private GameData _gameData;

        [Inject]
        public void Construct(GameData gameData)
        {
            _gameData = gameData;
        }

        public Ball SpawnBall()
        {
            return Instantiate(_gameData.ballPrefab, transform).GetComponent<Ball>();
        }
    }
}