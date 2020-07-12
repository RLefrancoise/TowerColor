using System.Linq;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(BrickDestroyEffectInstaller))]
    public class BrickDestroyEffect : MonoBehaviour
    {
        private GameData _gameData;
        private Color _color;
        private ParticleSystemRenderer _renderer;
        
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _renderer.material = _gameData.brickColors.First(b => b.color == value);
            }
        }

        [Inject]
        public void Construct(GameData gameData, ParticleSystemRenderer r)
        {
            _gameData = gameData;
            _renderer = r;
        }
        
        public class Factory : PlaceholderFactory<GameObject, BrickDestroyEffect>
        {}
    }
}