using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace TowerColor
{
    public class Ball : MonoBehaviour
    {
        /// <summary>
        /// The ball color
        /// </summary>
        private Color _color;

        private bool _isRainbow;

        private Sequence _fireSequence;

        /// <summary>
        /// Game data
        /// </summary>
        private GameData _gameData;

        [SerializeField] private new Renderer renderer;
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private new Collider collider;
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

        /*private void OnCollisionEnter(Collision other)
        {
            if(!IsFiring) return;
            
            if (other.collider.gameObject.CompareTag("Brick"))
            {
                var brick = other.collider.GetComponentInParent<Brick>();
                if(!brick) return;
                
                if(HasHitBrick) return;

                Debug.LogFormat("Ball hit brick {0}", brick.name);
                
                HasHitBrick = true;
                TouchedBrick?.Invoke(brick);
                
                //If brick color is different from the ball, repulse the ball
                if (brick.Color != _color)
                {
                    var repulse = transform.DOMove(transform.position + other.GetContact(0).normal * other.impulse.magnitude, 1f);
                    repulse.onComplete += () => Destroy(gameObject);
                }
            }
        }*/

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
                
                /*var repulse = transform.DOMove(transform.position + repulseVector * _gameData.ballRepulseDistance, _gameData.ballRepulseDuration)
                    .SetEase(AnimationCurve.Linear(0,0,1,1));
                repulse.onComplete += () => Destroy(gameObject);*/
            }
        }
    }
}