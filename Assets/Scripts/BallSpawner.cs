using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class BallSpawner : MonoBehaviour
    {
        private GameData _gameData;
        private Camera _playerCamera;

        #region MonoBehaviours

        private void Update()
        {
            transform.position = _playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.2f, _gameData.ballDistanceFromCamera));
        }

        #endregion
        
        [Inject]
        public void Construct(GameData gameData, Camera playerCamera)
        {
            _gameData = gameData;
            _playerCamera = playerCamera;
        }

        public Ball SpawnBall()
        {
            return Instantiate(_gameData.ballPrefab, transform).GetComponent<Ball>();
        }
    }
}