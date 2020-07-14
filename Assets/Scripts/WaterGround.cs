using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    /// <summary>
    /// Water ground. It drifts away bricks from the tower
    /// </summary>
    public class WaterGround : MonoBehaviour
    {
        /// <summary>
        /// Game data
        /// </summary>
        private GameData _gameData;
        
        /// <summary>
        /// Bricks hit points
        /// </summary>
        private List<Vector3> _bricksHitPoints;

        /// <summary>
        /// Collision events
        /// </summary>
        [SerializeField] private CollisionEvents collisionEvents;
        
        /// <summary>
        /// Bricks
        /// </summary>
        [SerializeField] private List<Rigidbody> bricks;

        [Inject]
        public void Construct(GameData gameData)
        {
            _gameData = gameData;
        }

        private void Awake()
        {
            _bricksHitPoints = new List<Vector3>();

            collisionEvents.CollisionEnter += OnCollisionEnter;
        }

        private void Update()
        {
            for (var i = 0; i < bricks.Count; ++i)
            {
                if (Vector3.Distance(bricks[i].position, transform.position) > _gameData.waterGroundMaxBrickDistance)
                {
                    bricks[i].velocity = Vector3.zero;
                    continue;
                }
                
                if (bricks[i].velocity.magnitude >= _gameData.waterGroundMaxBrickSpeed) continue;
                
                var forceVector = new Vector3(
                    _bricksHitPoints[i].x - transform.position.x, 
                    0f, 
                    _bricksHitPoints[i].z - transform.position.z).normalized;
                bricks[i].AddForce(_gameData.waterGroundForce * forceVector, ForceMode.Acceleration);
            }
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Brick"))
            {
                var brick = other.rigidbody.GetComponent<Brick>();
                brick.Destroyed += (b) => bricks.Remove(other.rigidbody);
                
                bricks.Add(other.rigidbody);
                _bricksHitPoints.Add(other.GetContact(0).point);
            }
        }
    }
}