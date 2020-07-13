using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace TowerColor.UI
{
    [RequireComponent(typeof(Graphic))]
    public class RainbowGraphic : RainbowEffect
    {
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