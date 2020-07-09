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
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Brick : MonoBehaviour
    {
        #region Fields
        
        /// <summary>
        /// The color of the brick
        /// </summary>
        private Color _color;

        /// <summary>
        /// The brick renderer. Used to change the color material
        /// </summary>
        [SerializeField] protected new Renderer renderer;

        /// <summary>
        /// The brick collider. Used to get brick dimensions.
        /// </summary>
        [SerializeField] protected new Collider collider;

        /// <summary>
        /// Rigid body. Used to enable / disable physics 
        /// </summary>
        [SerializeField] protected Rigidbody rigidBody;
        
        /// <summary>
        /// Game data
        /// </summary>
        private GameData _gameData;
        
        #endregion

        #region Properties
        
        /// <summary>
        /// Get or set the color of the brick
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                renderer.sharedMaterial = _gameData.brickColors.First(x => x.color == value);

            }
        }

        /// <summary>
        /// The height of the brick
        /// </summary>
        public float Height => collider.bounds.size.y;

        /// <summary>
        /// Brick center
        /// </summary>
        public Vector3 Center => collider.bounds.center;

        /// <summary>
        /// Brick bounds
        /// </summary>
        public Bounds Bounds => collider.bounds;
        
        /// <summary>
        /// Enable of disable physics
        /// </summary>
        public bool PhysicsEnabled
        {
            get => !rigidBody.isKinematic;
            set
            {
                rigidBody.isKinematic = !value;
                rigidBody.velocity = Vector3.zero;
                rigidBody.angularVelocity = Vector3.zero;
            }
        }
        
        #endregion

        #region Public Methods
        
        [Inject]
        public void Construct(GameData gameData)
        {
            _gameData = gameData;
        }
        
        public void SetActivated(bool activated)
        {
            if (activated)
            {
                Color = _color;
                rigidBody.constraints = RigidbodyConstraints.None;
            }
            else
            {
                renderer.sharedMaterial = _gameData.inactiveBrickColor;
                rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            }

            PhysicsEnabled = activated;
        }
        
        #endregion
    }
}
