using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// Base class for rainbow effect
    /// </summary>
    public abstract class RainbowEffect : MonoBehaviour
    {
        /// <summary>
        /// rainbow sequence
        /// </summary>
        private Color[] rainbow = { Color.red, Color.magenta, Color.blue, Color.cyan, Color.green, Color.yellow, };
        
        /// <summary>
        /// Current color
        /// </summary>
        private int _currentColor;
        
        /// <summary>
        /// Color tween
        /// </summary>
        private TweenerCore<Color, Color, ColorOptions> _colorTween;

        /// <summary>
        /// Rainbow speed
        /// </summary>
        [SerializeField] private float speed = 1f;

        protected virtual void OnDestroy()
        {
            _colorTween?.Kill();
        }

        protected virtual void OnEnable()
        {
            _currentColor = 0;
            ApplyColor(rainbow[_currentColor]);
            NextColor();
        }

        protected virtual void OnDisable()
        {
            _colorTween?.Kill();
            _colorTween = null;
        }

        /// <summary>
        /// Go to next color
        /// </summary>
        protected virtual void NextColor()
        {
            _currentColor++;
            _colorTween = LerpColor(rainbow[_currentColor % rainbow.Length], 1f / speed);
            _colorTween.onComplete += NextColor;
        }

        /// <summary>
        /// Apply given color
        /// </summary>
        /// <param name="c">Color</param>
        protected abstract void ApplyColor(Color c);
        
        /// <summary>
        /// Lerp color
        /// </summary>
        /// <param name="endColor">End color</param>
        /// <param name="duration">Duration</param>
        /// <returns></returns>
        protected abstract TweenerCore<Color, Color, ColorOptions> LerpColor(Color endColor, float duration);
    }
}