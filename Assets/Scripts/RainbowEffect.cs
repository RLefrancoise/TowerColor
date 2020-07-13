using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace TowerColor
{
    public abstract class RainbowEffect : MonoBehaviour
    {
        private Color[] rainbow = { Color.red, Color.magenta, Color.blue, Color.cyan, Color.green, Color.yellow, };
        private int _currentColor;
        private TweenerCore<Color, Color, ColorOptions> _colorTween;

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

        protected virtual void NextColor()
        {
            _currentColor++;
            _colorTween = LerpColor(rainbow[_currentColor % rainbow.Length], 1f / speed);
            _colorTween.onComplete += NextColor;
        }

        protected abstract void ApplyColor(Color c);
        protected abstract TweenerCore<Color, Color, ColorOptions> LerpColor(Color endColor, float duration);
    }
}