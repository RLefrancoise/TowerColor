using Framework.UI;
using TMPro;
using UnityEngine;

namespace TowerColor.UI
{
    public class FeverMessage : PoppingMessageWithAnimator
    {
        [SerializeField] private TMP_Text text;
        
        private int _rainbowBallsCount;
        
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