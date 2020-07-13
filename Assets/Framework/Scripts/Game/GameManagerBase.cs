using Framework.Views;
using UnityEngine;
using Zenject;

namespace Framework.Game
{
    /// <summary>
    /// Base class for the game manager
    /// </summary>
    public abstract class GameManagerBase : MonoBehaviour, IGameManager
    {
        private GameState _currentState;
        private IViewManager _viewManager;

        public ILevelManager LevelManager { get; private set; }
        
        public GameState CurrentState
        {
            get => _currentState;
            private set
            {
                _currentState = value;
                OnStateChanged(value);
            }
        }

        protected virtual void Start()
        {
            ChangeState(GameState.Menu, true);
        }
        
        [Inject]
        public void Construct(IViewManager viewManager, ILevelManager levelManager)
        {
            _viewManager = viewManager;
            LevelManager = levelManager;
        }

        public void ChangeState(GameState state, bool skipFade = false)
        {
            _viewManager.ShowGameState(state, () =>
            {
                CurrentState = state;
            }, skipFade);
        }
        
        protected virtual void OnStateChanged(GameState state) {}
    }
}