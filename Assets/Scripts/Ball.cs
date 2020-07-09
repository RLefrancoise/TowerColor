using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class Ball : MonoBehaviour
    {
        /// <summary>
        /// The ball color
        /// </summary>
        private Color _color;

        /// <summary>
        /// Game data
        /// </summary>
        private GameData _gameData;

        [SerializeField] private new Renderer renderer;
        
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
        /// Is the ball being fired ?
        /// </summary>
        public bool IsFiring { get; private set; }

        /// <summary>
        /// Ball has reached a brick
        /// </summary>
        public event Action<Brick> TouchedBrick; 
        
        [Inject]
        public void Construct(GameData gameData)
        {
            _gameData = gameData;
        }

        public void FireTo(Brick brick)
        {
            IsFiring = true;
            
            var tween = transform.DOMove(brick.Center, _gameData.ballFireDuration).SetEase(AnimationCurve.Linear(0,0,1,1));
            tween.onComplete += () =>
            {
                IsFiring = false;
                TouchedBrick?.Invoke(brick);
            };
        }
    }
}