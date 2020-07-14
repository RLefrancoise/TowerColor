using TMPro;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    /// <summary>
    /// Interface for ball spawner
    /// </summary>
    public interface IBallSpawner
    {
        /// <summary>
        /// Spawn ball
        /// </summary>
        /// <returns></returns>
        Ball SpawnBall();
        
        /// <summary>
        /// Set remaining ball count
        /// </summary>
        /// <param name="remainingBalls">Ball count</param>
        void SetRemainingBalls(int remainingBalls);

        /// <summary>
        /// Activate spawner or not
        /// </summary>
        /// <param name="activate">Activate</param>
        void Activate(bool activate);
    }
    
    /// <summary>
    /// Ball spawner
    /// </summary>
    public class BallSpawner : MonoBehaviour, IBallSpawner
    {
        /// <summary>
        /// Balls left text
        /// </summary>
        [SerializeField] private TMP_Text ballsLeft;
        
        /// <summary>
        /// Game data
        /// </summary>
        private GameData _gameData;
        
        /// <summary>
        /// Player camera
        /// </summary>
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