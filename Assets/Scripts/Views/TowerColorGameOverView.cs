using Framework.Views.Default;
using UnityEngine;
using Zenject;

namespace TowerColor.Views
{
    public class TowerColorGameOverView : DefaultGameOverView
    {
        private ISoundPlayer _soundPlayer;
        
        [SerializeField] private Animator levelFailedAnimation;
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