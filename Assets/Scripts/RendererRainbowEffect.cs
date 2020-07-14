using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// Rainbow effect for renderer
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class RendererRainbowEffect : RainbowEffect
    {
        /// <summary>
        /// Renderer
        /// </summary>
        [SerializeField] private new Renderer renderer; 
        
        protected override void ApplyColor(Color c)
        {
            renderer.material.color = c;
        }

        protected override TweenerCore<Color, Color, ColorOptions> LerpColor(Color endColor, float duration)
        {
            return renderer.material.DOColor(endColor, duration);
        }
    }
}