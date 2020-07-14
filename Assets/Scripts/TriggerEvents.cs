using System;
using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// Trigger events dispatcher
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TriggerEvents : MonoBehaviour
    {
        /// <summary>
        /// Trigger entered
        /// </summary>
        public event Action<Collider> TriggerEntered;
        
        /// <summary>
        /// Trigger exited
        /// </summary>
        public event Action<Collider> TriggerExited;
        
        /// <summary>
        /// Trigger stayed
        /// </summary>
        public event Action<Collider> TriggerStayed;

        private void OnTriggerEnter(Collider other)
        {
            TriggerEntered?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExited?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            TriggerStayed?.Invoke(other);
        }
    }
}