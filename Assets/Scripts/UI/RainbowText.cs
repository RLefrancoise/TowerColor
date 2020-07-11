using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TowerColor.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class RainbowText : MonoBehaviour
    {
        private Color[] rainbow = { Color.red, Color.magenta, Color.blue, Color.cyan, Color.green, Color.yellow, };
        
        [SerializeField] private float speed = 1f;
        [SerializeField] private TMP_Text text;

        private int _currentColor;
        private float _currentLerp;

        private void Start()
        {
            _currentColor = 0;
            text.color = rainbow[_currentColor];
            NextColor();
        }

        private void NextColor()
        {
            _currentColor++;
            text.DOColor(rainbow[_currentColor % rainbow.Length], 1f / speed).onComplete += NextColor;
        }
    }
}