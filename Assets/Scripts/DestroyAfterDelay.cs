using UnityEngine;

namespace TowerColor
{
    public class DestroyAfterDelay : MonoBehaviour
    {
        [SerializeField] private float delay = 1f;

        private void Start()
        {
            Destroy(gameObject, delay);
        }
    }
}