using System;
using UnityEngine;

namespace TowerColor
{
    [RequireComponent(typeof(Collider))]
    public class TriggerEvents : MonoBehaviour
    {
        public event Action<Collider> TriggerEntered;
        public event Action<Collider> TriggerExited;
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