using Framework.UI;
using TMPro;
using TowerColor.UI.Installers;
using UnityEngine;
using Zenject;

namespace TowerColor.UI
{
    /// <summary>
    /// Ball gained message
    /// </summary>
    [RequireComponent(typeof(BallGainedMessageInstaller))]
    public class BallGainedMessage : PoppingMessageWithAnimator
    {
        /// <summary>
        /// Text
        /// </summary>
        private TMP_Text _text;
        
        /// <summary>
        /// Balls gained
        /// </summary>
        private int _ballsGained;
        
        /// <summary>
        /// Balls gained
        /// </summary>
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