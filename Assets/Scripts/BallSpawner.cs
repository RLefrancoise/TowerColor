using TMPro;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    public interface IBallSpawner
    {
        Ball SpawnBall();
        void SetRemainingBalls(int remainingBalls);

        void Activate(bool activate);
    }
    
    public class BallSpawner : MonoBehaviour, IBallSpawner
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

        public void Activate(bool activate)
        {
            gameObject.SetActive(activate);
        }
    }
}