using UnityEngine;

namespace TowerColor
{
    public class ScalePingPong : MonoBehaviour
    {
        [SerializeField] private Vector3 minScale;
        [SerializeField] private Vector3 maxScale;
        [SerializeField] private float speed = 1f;

        private void Update()
        {
            transform.localScale = Vector3.Lerp(minScale, maxScale, Mathf.PingPong(Time.time * speed, 1f));
        }
    }
}