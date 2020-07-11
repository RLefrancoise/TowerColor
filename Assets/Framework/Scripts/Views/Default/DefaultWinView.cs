using Framework.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Framework.Views.Default
{
    /// <summary>
    /// Default win view
    /// </summary>
    public class DefaultWinView : WinView
    {
        private IGameManager _gameManager;
        
        [SerializeField] private Button continueButton;
        
        [Inject]
        public void Construct(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }
        
        protected override void OnShow()
        {
            base.OnShow();
            
            continueButton.onClick.AddListener(ClickOnContinue);
        }

        private void ClickOnContinue()
        {
            _gameManager.LevelManager.CurrentLevel++;
        }
    }
}