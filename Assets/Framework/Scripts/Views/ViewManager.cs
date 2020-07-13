using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Framework.Game;

namespace Framework.Views
{
    public interface IViewManager
    {
        void ShowGameState(GameState state, Action showOverCallback = null, bool skipFade = false);
    }
    
    public class ViewManager : MonoBehaviour, IViewManager
    {
        [SerializeField] private GameObject menuViewPrefab;
        [SerializeField] private GameObject startViewPrefab;
        [SerializeField] private GameObject playingViewPrefab;
        [SerializeField] private GameObject winViewPrefab;
        [SerializeField] private GameObject gameOverViewPrefab;
        
        private IEnumerable<IView> _views;
        private IView _currentView;

        [Inject]
        public void Construct(DiContainer container)
        {
            var menu = container.InstantiatePrefab(menuViewPrefab, transform).GetComponent<MenuView>();
            var start = container.InstantiatePrefab(startViewPrefab, transform).GetComponent<StartView>();
            var playing = container.InstantiatePrefab(playingViewPrefab, transform).GetComponent<PlayingView>();
            var win = container.InstantiatePrefab(winViewPrefab, transform).GetComponent<WinView>();
            var gameOver = container.InstantiatePrefab(gameOverViewPrefab, transform).GetComponent<GameOverView>();
            
            _views = new List<IView>{menu, start, playing, win, gameOver};
            
            foreach (var view in _views)
            {
                view.Hide(true);
            }
        }

        public void ShowGameState(GameState state, Action showOverCallback = null, bool skipFade = false)
        {
            if (_currentView != null)
            {
                _currentView.Hide(skipFade, () =>
                {
                    _currentView = _views.First(v => v.State == state);
                    _currentView.Show(skipFade, showOverCallback);
                });
            }
            else
            {
                _currentView = _views.First(v => v.State == state);
                _currentView.Show(skipFade, showOverCallback);
            }
        }
    }
}