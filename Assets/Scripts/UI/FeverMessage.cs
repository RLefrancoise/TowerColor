using Framework.UI;
using TMPro;
using UnityEngine;

namespace TowerColor.UI
{
    /// <summary>
    /// Fever message
    /// </summary>
    public class FeverMessage : PoppingMessageWithAnimator
    {
        /// <summary>
        /// Text
        /// </summary>
        [SerializeField] private TMP_Text text;
        
        /// <summary>
        /// Rainbow ball count
        /// </summary>
        private int _rainbowBallsCount;
        
        /// <summary>
        /// Rainbow ball count
        /// </summary>
        public int RainbowBallsCount
        {
            get => _rainbowBallsCount;
            set
            {
                _rainbowBallsCount = value;
                text.text = value > 1 ? $"{value} Rainbow Balls!" : $"{value} Rainbow Ball!";
            }
        }
        
        public class Factory : PoppingMessageFactory<FeverMessage>
        {}
    }
}