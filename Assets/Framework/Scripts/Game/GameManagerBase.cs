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
        private ViewManager _viewManager;

        public LevelManager LevelManager { get; private set; }
        
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
            ChangeState(GameState.Menu);
        }
        
        [Inject]
        public void Construct(ViewManager viewManager, LevelManager levelManager)
        {
            _viewManager = viewManager;
            LevelManager = levelManager;
        }

        public void ChangeState(GameState state)
        {
            _viewManager.ShowGameState(state, () =>
            {
                CurrentState = state;
            });
        }
        
        protected virtual void OnStateChanged(GameState state) {}
    }
}