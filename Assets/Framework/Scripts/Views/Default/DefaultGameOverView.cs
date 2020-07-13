using Framework.Game;
using TMPro;
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
        
        [SerializeField] private TMP_Text currentLevel;
        [SerializeField] private Button continueButton;
        
        [Inject]
        public void Construct(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }
        
        protected override void OnShow()
        {
            base.OnShow();
            
            currentLevel.text = $"Level {_gameManager.LevelManager.CurrentLevel}";
            continueButton.onClick.AddListener(ClickOnContinue);
        }

        protected virtual void ClickOnContinue()
        {
            _gameManager.LevelManager.ReloadLevel();
        }
    }
}