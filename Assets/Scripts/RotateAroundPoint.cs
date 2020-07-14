using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// Rotate around a given point
    /// </summary>
    public class RotateAroundPoint : MonoBehaviour
    {
        /// <summary>
        /// Point
        /// </summary>
        [SerializeField] private Transform point;
        
        /// <summary>
        /// Axis in local space
        /// </summary>
        [SerializeField] private Vector3 axis;
        
        /// <summary>
        /// Speed
        /// </summary>
        [SerializeField] private float speed = 1f;
        
        private void Update()
        {
            transform.RotateAround(point.position, transform.TransformDirection(axis), Time.deltaTime * speed);
        }
    }
}