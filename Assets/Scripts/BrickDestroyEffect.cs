using UnityEngine;
using Zenject;

namespace TowerColor
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BrickDestroyEffect : MonoBehaviour
    {
        private Color _color;
        private ParticleSystemRenderer _renderer;
        
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