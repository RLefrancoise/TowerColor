using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class WaterGround : MonoBehaviour
    {
        private GameData _gameData;
        private List<Vector3> _bricksHitPoints;

        [SerializeField] private CollisionEvents collisionEvents;
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
                bricks.Add(other.rigidbody);
                _bricksHitPoints.Add(other.GetContact(0).point);
            }
        }
    }
}