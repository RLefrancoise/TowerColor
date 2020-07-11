using Framework.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Framework.Views.Default
{
    /// <summary>
    /// Default game over view
    /// </summary>
    public class DefaultGameOverView : GameOverView
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
            _gameManager.LevelManager.ReloadLevel();
        }
    }
}