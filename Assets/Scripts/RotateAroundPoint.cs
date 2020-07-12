using UnityEngine;

namespace TowerColor
{
    public class RotateAroundPoint : MonoBehaviour
    {
        [SerializeField] private Transform point;
        [SerializeField] private Vector3 axis;
        [SerializeField] private float speed = 1f;
        
        private void Update()
        {
            transform.RotateAround(point.position, transform.TransformDirection(axis), Time.deltaTime * speed);
        }
    }
}