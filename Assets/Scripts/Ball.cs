using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace TowerColor
{
    /// <summary>
    /// The ball
    /// </summary>
    public class Ball : MonoBehaviour
    {
        /// <summary>
        /// The ball color
        /// </summary>
        private Color _color;

        /// <summary>
        /// Is rainbow ball ?
        /// </summary>
        private bool _isRainbow;

        /// <summary>
        /// Fire sequence
        /// </summary>
        private Sequence _fireSequence;

        /// <summary>
        /// Game data
        /// </summary>
        private GameData _gameData;

        /// <summary>
        /// Renderer
        /// </summary>
        [SerializeField] private new Renderer renderer;
        
        /// <summary>
        /// Rigidbody
        /// </summary>
        [SerializeField] private Rigidbody rigidBody;
        
        /// <summary>
        /// Collider
        /// </summary>
        [SerializeField] private new Collider collider;
        
        /// <summary>
        /// Rainbow effect
        /// </summary>
        [SerializeField] private RendererRainbowEffect rainbowEffect;
        
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
        /// Is rainbow ?
        /// </summary>
        public bool IsRainbow
        {
            get => _isRainbow;
            set
            {
                _isRainbow = value;
                rainbowEffect.enabled = value;
            }
        }

        /// <summary>
        /// Is the ball being fired ?
        /// </summary>
        public bool IsFiring { get; private set; }
        
        /// <summary>
        /// Does the ball hit a brick ?
        /// </summary>
        public bool HasHitBrick { get; private set; }

        /// <summary>
        /// Ball has reached a brick
        /// </summary>
        public event Action<Brick> TouchedBrick;

        [Inject]
        public void Construct(GameData gameData)
        {
            _gameData = gameData;
            IsRainbow = false;
        }

        private void OnDestroy()
        {
            _fireSequence?.Kill();
        }

        /// <summary>
        /// Fire ball
        /// </summary>
        /// <param name="brick">Brick to target</param>
        /// <param name="hitPoint">Hit point</param>
        /// <param name="hitNormal">Hit normal</param>
        public void FireTo(Brick brick, Vector3 hitPoint, Vector3 hitNormal)
        {
            IsFiring = true;

            _fireSequence = DOTween.Sequence();
            
            //Move to brick with a little jump
            _fireSequence.Insert(0, transform.DOJump(
                hitPoint, 
                _gameData.ballJumpForce, 
                1, 
                _gameData.ballFireDuration).SetEase(AnimationCurve.Linear(0,0,1,1)));
            
            //Rotate on itself
            _fireSequence.Insert(0, transform.DOLocalRotate(new Vector3(360f * -_gameData.ballTurnNumber, 0f, 0f), _gameData.ballFireDuration,
                RotateMode.FastBeyond360));
            
            _fireSequence.onComplete += () => OnFireComplete(brick, hitPoint, hitNormal);
        }

        /// <summary>
        /// When fire is over
        /// </summary>
        /// <param name="brick">The targeted brick</param>
        /// <param name="hitPoint">The hit point</param>
        /// <param name="repulseVector">The repulse vector in case the ball color is different from the brick</param>
        private void OnFireComplete(Brick brick, Vector3 hitPoint, Vector3 repulseVector)
        {
            IsFiring = false;

            //Apply some force on the brick to have kind of impact effect
            //Yes we could have use physics to do that, but it's too unpredictable sometimes...
            brick.ApplyImpactForce(-repulseVector * 10f, hitPoint);
            
            HasHitBrick = true;
            TouchedBrick?.Invoke(brick);
                
            //If not same color, repulse ball
            if (_color != brick.Color)
            {
                collider.enabled = true;
                
                rigidBody.AddForce(repulseVector * _gameData.ballRepulseDistance, ForceMode.Impulse);
                rigidBody.useGravity = true;
                rigidBody.isKinematic = false;
                
                Destroy(gameObject, _gameData.ballRepulseDuration);
            }
        }
    }
}