using Framework.UI;
using TMPro;
using TowerColor.UI.Installers;
using UnityEngine;
using Zenject;

namespace TowerColor.UI
{
    [RequireComponent(typeof(BallGainedMessageInstaller))]
    public class BallGainedMessage : PoppingMessageWithAnimator
    {
        private TMP_Text _text;
        private int _ballsGained;
        
        public int BallsGained
        {
            get => _ballsGained;
            set
            {
                _ballsGained = value;
                _text.text = $"Ball +{value}";
            }
        }

        [Inject]
        public void Construct(TMP_Text text)
        {
            _text = text;
        }
        
        public class Factory : PoppingMessageFactory<BallGainedMessage>
        {}
    }
}