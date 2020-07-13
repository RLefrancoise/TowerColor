using Framework.Game;
using TMPro;
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

        [SerializeField] private TMP_Text currentLevel;
        [SerializeField] private Button playButton;
        
        [Inject]
        public void Construct(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }
        
        protected override void OnShow()
        {
            base.OnShow();

            currentLevel.text = $"Level {_gameManager.LevelManager.CurrentLevel}";
            playButton.onClick.AddListener(ClickOnPlay);
        }

        protected virtual void ClickOnPlay()
        {
            _gameManager.ChangeState(GameState.Start);
        }
    }
}