using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    /// <summary>
    /// A brick is an element of the tower. It has a color and a shape.
    /// </summary>
    [RequireComponent(typeof(BrickInstaller))]
    public abstract class Brick : MonoBehaviour
    {
        /// <summary>
        /// The color of the brick
        /// </summary>
        protected Color _color;

        /// <summary>
        /// The brick renderer. Used to change the color material
        /// </summary>
        protected Renderer _renderer;

        /// <summary>
        /// The brick collider. Used to get brick dimensions.
        /// </summary>
        protected Collider _collider;
        
        /// <summary>
        /// Game data
        /// </summary>
        private GameData _gameData;

        /// <summary>
        /// Get or set the color of the brick
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _renderer.sharedMaterial = _gameData.brickColors.First(x => x.color == value);

            }
        }

        /// <summary>
        /// The height of the brick
        /// </summary>
        public float Height => _collider.bounds.size.y;

        [Inject]
        public void Construct(GameData gameData, Renderer r, Collider c)
        {
            _gameData = gameData;
            _renderer = r;
            _collider = c;
        }
    }
}
