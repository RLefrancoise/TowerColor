using UnityEngine;
using Zenject;

namespace TowerColor
{
    /// <summary>
    /// Brick destroy effect
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class BrickDestroyEffect : MonoBehaviour
    {
        /// <summary>
        /// Color
        /// </summary>
        private Color _color;
        
        /// <summary>
        /// Renderer
        /// </summary>
        private ParticleSystemRenderer _renderer;
        
        /// <summary>
        /// Color
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _renderer.material.color = value; 
            }
        }

        private void Awake()
        {
            _renderer = GetComponent<ParticleSystemRenderer>();
        }

        public class Factory : PlaceholderFactory<GameObject, BrickDestroyEffect>
        {}
    }
}