using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace TowerColor.UI
{
    /// <summary>
    /// Rainbow effect for Unity UI Graphic
    /// </summary>
    [RequireComponent(typeof(Graphic))]
    public class RainbowGraphic : RainbowEffect
    {
        /// <summary>
        /// Graphic element
        /// </summary>
        [SerializeField] private Graphic graphic;
        
        protected override void ApplyColor(Color c)
        {
            graphic.color = c;
        }

        protected override TweenerCore<Color, Color, ColorOptions> LerpColor(Color endColor, float duration)
        {
            return graphic.DOColor(endColor, duration);
        }
    }
}