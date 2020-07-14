using System;
using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// Collision events dispatcher
    /// </summary>
    public class CollisionEvents : MonoBehaviour
    {
        /// <summary>
        /// When collision enter
        /// </summary>
        public event Action<Collision> CollisionEnter;
        
        private void OnCollisionEnter(Collision other)
        {
            CollisionEnter?.Invoke(other);    
        }
    }
}