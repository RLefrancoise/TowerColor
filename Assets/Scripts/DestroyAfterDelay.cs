using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// Destroy after seconds
    /// </summary>
    public class DestroyAfterDelay : MonoBehaviour
    {
        /// <summary>
        /// Delay
        /// </summary>
        [SerializeField] private float delay = 1f;

        private void Start()
        {
            Destroy(gameObject, delay);
        }
    }
}