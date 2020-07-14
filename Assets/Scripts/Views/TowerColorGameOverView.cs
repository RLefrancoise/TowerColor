using Framework.Views.Default;
using UnityEngine;
using Zenject;

namespace TowerColor.Views
{
    /// <summary>
    /// Tower color game over view
    /// </summary>
    public class TowerColorGameOverView : DefaultGameOverView
    {
        /// <summary>
        /// Sound player
        /// </summary>
        private ISoundPlayer _soundPlayer;
        
        /// <summary>
        /// Level failed animation
        /// </summary>
        [SerializeField] private Animator levelFailedAnimation;
        
        /// <summary>
        /// Game over sound
        /// </summary>
        [SerializeField] private AudioSource gameOverSound;

        [Inject]
        public void Construct(ISoundPlayer soundPlayer)
        {
            _soundPlayer = soundPlayer;
        }
        
        protected override void OnShow()
        {
            base.OnShow();
            levelFailedAnimation.Play("LevelFailed");
            
            _soundPlayer.PlaySound(gameOverSound);
        }
    }
}