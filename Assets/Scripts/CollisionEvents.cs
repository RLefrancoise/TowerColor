using System;
using UnityEngine;

namespace TowerColor
{
    public class CollisionEvents : MonoBehaviour
    {
        public event Action<Collision> CollisionEnter;
        
        private void OnCollisionEnter(Collision other)
        {
            CollisionEnter?.Invoke(other);    
        }
    }
}