using Framework.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Framework.Views.Default
{
    /// <summary>
    /// Default menu view
    /// </summary>
    public class DefaultMenuView : MenuView
    {
        private IGameManager _gameManager;
        
        [SerializeField] private Button playButton;
        
        [Inject]
        public void Construct(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }
        
        protected override void OnShow()
        {
            base.OnShow();
            
            playButton.onClick.AddListener(ClickOnPlay);
        }

        private void ClickOnPlay()
        {
            _gameManager.ChangeState(GameState.Start);
        }
    }
}