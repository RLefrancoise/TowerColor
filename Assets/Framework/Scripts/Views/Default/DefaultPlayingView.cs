using Framework.Game;
using TMPro;
using UnityEngine;
using Zenject;

namespace Framework.Views.Default
{
    public class DefaultPlayingView : PlayingView
    {
        private IGameManager _gameManager;
        [SerializeField] private TMP_Text currentLevel;
        
        [Inject]
        public void Construct(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }
        
        protected override void OnShow()
        {
            base.OnShow();
            currentLevel.text = $"Level {_gameManager.LevelManager.CurrentLevel}";
        }
    }
}