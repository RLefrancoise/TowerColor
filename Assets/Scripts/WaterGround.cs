using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class WaterGround : MonoBehaviour
    {
        private GameData _gameData;
        private List<Vector3> _bricksHitPoints;

        [SerializeField] private List<Rigidbody> bricks;

        [Inject]
        public void Construct(GameData gameData)
        {
            _gameData = gameData;
        }
        
        private void Awake()
        {
            _bricksHitPoints = new List<Vector3>();
        }

        private void Update()
        {
            for (var i = 0; i < bricks.Count; ++i)
            {
                bricks[i].AddForce((_bricksHitPoints[i] - transform.position).normalized * _gameData.waterGroundForce, ForceMode.Force);
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