using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
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
        /// Game data
        /// </summary>
        private GameData _gameData;

        /// <summary>
        /// Start position of the brick. Used to check if the brick is still in place or not
        /// </summary>
        [ShowNonSerializedField] private Vector3 _startPosition;
        
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
        /// Is the brick activated ?
        /// </summary>
        public bool IsActivated { get; private set; }
        
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

        /// <summary>
        /// Does the brick has surrounding bricks ?
        /// </summary>
        public bool HasSurroundingBricks => SurroundingBricks.Any();

        /// <summary>
        /// Get surrounding bricks of the brick
        /// </summary>
        public IEnumerable<Brick> SurroundingBricks
        {
            get
            {
                var hits = new RaycastHit[10];
                var size = Physics.BoxCastNonAlloc(
                    Center, 
                    Bounds.extents * 1.25f, 
                    transform.up, 
                    hits, 
                    transform.rotation, 
                    Bounds.size.y, 
                    LayerMask.GetMask("Brick"));
                hits = hits.Take(size).ToArray();
                
                return hits.Where(h => h.collider.GetComponentInParent<Brick>() != null)
                    .Select(h => h.collider.GetComponentInParent<Brick>());
            }
        }

        [ShowNativeProperty] public bool IsStillInPlace
        {
            get
            {
                return collider.bounds.Contains(_startPosition) && Vector3.Dot(transform.up, Vector3.up) >= 0.95f;
            }
        }
        
        public bool IsInWater { get; private set; }

        #endregion

        public event Action<Brick> Destroyed;
        
        #region MonoBehaviour

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke(this);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Water"))
            {
                IsInWater = true;
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
            if(!IsStillInPlace) return;
            
            IsActivated = activated;
            
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

        /*private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var bounds = new Bounds(_startPosition + Vector3.up * Height / 2f, Bounds.size);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }*/
    }
}
