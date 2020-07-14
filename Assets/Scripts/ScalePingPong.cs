using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// Scale ping pong
    /// </summary>
    public class ScalePingPong : MonoBehaviour
    {
        /// <summary>
        /// Min scale
        /// </summary>
        [SerializeField] private Vector3 minScale;
        
        /// <summary>
        /// Max scale
        /// </summary>
        [SerializeField] private Vector3 maxScale;
        
        /// <summary>
        /// Speed
        /// </summary>
        [SerializeField] private float speed = 1f;

        private void Update()
        {
            transform.localScale = Vector3.Lerp(minScale, maxScale, Mathf.PingPong(Time.time * speed, 1f));
        }
    }
}