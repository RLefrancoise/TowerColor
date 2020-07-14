using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

namespace TowerColor.UI
{
    /// <summary>
    /// Rainbow effect for TMP_Text
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class RainbowText : RainbowEffect
    {
        /// <summary>
        /// Text
        /// </summary>
        [SerializeField] private TMP_Text text;

        protected override void ApplyColor(Color c)
        {
            text.color = c;
        }

        protected override TweenerCore<Color, Color, ColorOptions> LerpColor(Color endColor, float duration)
        {
            return text.DOColor(endColor, duration);
        }
    }
}