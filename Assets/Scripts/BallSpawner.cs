using TMPro;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private TMP_Text ballsLeft;
        
        private GameData _gameData;
        private Camera _playerCamera;
        
        #region MonoBehaviours

        private void Update()
        {
            transform.position = _playerCamera.ViewportToWorldPoint(new Vector3(
                _gameData.ballPositionOnScreen.x, 
                _gameData.ballPositionOnScreen.y, 
                _gameData.ballDistanceFromCamera));
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

        public void SetRemainingBalls(int remainingBalls)
        {
            ballsLeft.text = remainingBalls.ToString();
        }
    }
}