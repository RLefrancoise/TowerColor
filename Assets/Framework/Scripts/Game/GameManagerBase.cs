using Framework.Views;
using UnityEngine;
using Zenject;

namespace Framework.Game
{
    /// <summary>
    /// Base class for the game manager
    /// </summary>
    public abstract class GameManagerBase : MonoBehaviour
    {
        private ViewManager _viewManager;
        private LevelManager _levelManager;
        private GameState _currentState;

        public LevelManager LevelManager => _levelManager;
        
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
            _levelManager = levelManager;
        }

        protected void ChangeState(GameState state)
        {
            _viewManager.ShowGameState(state, () =>
            {
                CurrentState = state;
            });
        }
        
        protected virtual void OnStateChanged(GameState state) {}
    }
}