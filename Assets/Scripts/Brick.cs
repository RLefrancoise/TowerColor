using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using NaughtyAttributes;
using UniRx.Async;
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

        private TweenerCore<Color, Color, ColorOptions> _lerpColorTween;

        /// <summary>
        /// Haptic manager
        /// </summary>
        private IHapticManager _hapticManager;
        
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
        /// Brick velocity
        /// </summary>
        public Vector3 Velocity => rigidBody.velocity;

        /// <summary>
        /// Is the brick activated ?
        /// </summary>
        [ShowNativeProperty]
        public bool IsActivated { get; private set; } = true;
        
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

        [ShowNativeProperty] public bool IsStillInPlace
        {
            get
            {
                return collider.bounds.Contains(_startPosition) 
                       /*&& Vector3.Dot(transform.up, Vector3.up) >= 0.95f*/;
            }
        }
        
        /// <summary>
        /// Has the brick fell in water ? Used to push it away from the tower
        /// </summary>
        [ShowNativeProperty] public bool IsInWater { get; private set; }
        
        /// <summary>
        /// Has brick fell on the platform ? Used to push it away from the tower
        /// </summary>
        [ShowNativeProperty] public bool HasFellOnPlatform { get; private set; }

        #endregion

        public event Action<Brick> Destroyed;
        
        #region MonoBehaviour

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void OnDestroy()
        {
            _lerpColorTween?.Kill();
            Destroyed?.Invoke(this);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Water"))
            {
                IsInWater = true;
            }
            else if (other.gameObject.CompareTag("Platform") && !IsStillInPlace)
            {
                HasFellOnPlatform = true;
            }
        }

        #endregion
        
        #region Public Methods
        
        [Inject]
        public void Construct(GameData gameData, IHapticManager hapticManager)
        {
            _gameData = gameData;
            _hapticManager = hapticManager;
        }

        public void Break()
        {
            //Vibrate
            _hapticManager.Vibrate();
            
            //Spawn effect
            var effect = Instantiate(_gameData.brickDestroyEffect).GetComponent<BrickDestroyEffect>();
            
            //Set color
            effect.Color = Color;

            //Place
            effect.transform.position = Center;
            effect.transform.rotation = transform.rotation;
                    
            Destroy(gameObject);
        }
        
        public void SetActivated(bool activated)
        {
            IsActivated = activated;
            
            if (activated)
            {
                Color = _color;
                rigidBody.constraints = RigidbodyConstraints.None;
            }
            else
            {
                if (HasFellOnPlatform || IsInWater)
                {
                    Debug.LogFormat("Brick {0} has fell on platform or is in water, cannot change activate state", name);
                    return;
                } 
                
                renderer.sharedMaterial = _gameData.inactiveBrickColor;
                rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            }

            PhysicsEnabled = activated;
        }

        /// <summary>
        /// Get surrounding bricks of the brick
        /// </summary>
        /// <returns></returns>
        public List<Brick> GetSurroundingBricks(bool takeNonActive = true)
        {
            var bricks = new List<Brick>();

            var upBrick = GetAdjacentBrick(transform.up, takeNonActive);
            var downBrick = GetAdjacentBrick(-transform.up, takeNonActive);
            var leftBrick = GetAdjacentBrick(-transform.right, takeNonActive);
            var rightBrick = GetAdjacentBrick(transform.right, takeNonActive);
            var forwardBrick = GetAdjacentBrick(transform.forward, takeNonActive);
            var backBrick = GetAdjacentBrick(-transform.forward, takeNonActive);
            
            if(upBrick) bricks.Add(upBrick);
            if(downBrick) bricks.Add(downBrick);
            if(leftBrick) bricks.Add(leftBrick);
            if(rightBrick) bricks.Add(rightBrick);
            if(forwardBrick) bricks.Add(forwardBrick);
            if(backBrick) bricks.Add(backBrick);

            return bricks;
        }

        public void ApplyImpactForce(Vector3 force, Vector3 hitPoint)
        {
            rigidBody.AddForceAtPosition(force, hitPoint, ForceMode.Force);
        }

        public async UniTask LerpColor(Color to, float duration)
        {
            //Kill tween if needed
            _lerpColorTween?.Kill();
            
            _color = to;
            
            //Lerp the color
            _lerpColorTween = renderer.material.DOColor(to, duration);
            await _lerpColorTween.ToUniTask();

            _lerpColorTween = null;
        }
        
        #endregion

        #region Private Methods
        
        private Brick GetAdjacentBrick(Vector3 rayDirection, bool takeNonActive = true)
        {
            if (Physics.Raycast(Center, rayDirection, out var hit, Bounds.size.magnitude, LayerMask.GetMask("Brick")))
            {
                var brick = hit.collider.GetComponentInParent<Brick>();
                if(brick.IsActivated || (!brick.IsActivated && takeNonActive))
                    return brick;
            }

            return null;
        }
        
        #if UNITY_EDITOR
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            var bricks = GetSurroundingBricks();
            foreach (var brick in bricks)
            {
                Gizmos.DrawWireCube(brick.Center, brick.Bounds.size);
            }
        }
        
        #endif
        
        #endregion
    }
}
