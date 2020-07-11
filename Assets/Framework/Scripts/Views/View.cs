using System;
using DG.Tweening;
using Framework.Game;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Framework.Views
{
    public interface IView
    {
        GameState State { get; }

        void Show(bool skipFade = false, Action fadeCallback = null);
        void Hide(bool skipFade = false, Action fadeCallback = null);
    }
    
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class View : MonoBehaviour, IView
    {
        private CanvasGroup _canvasGroup;
        
        [SerializeField] protected float fadeDuration = .2f;
        
        public abstract GameState State { get; }
        
        [Inject]
        public void Construct(CanvasGroup canvasGroup)
        {
            _canvasGroup = canvasGroup;
        }
        
        public void Show(bool skipFade = false, Action fadeCallback = null)
        {
            gameObject.SetActive(true);

            if (skipFade)
            {
                _canvasGroup.alpha = 1f;
                OnShow();
            }
            else
            {
                var tween = _canvasGroup.DOFade(1f, fadeDuration);
                tween.onComplete += () =>
                {
                    OnShow();
                    fadeCallback?.Invoke();
                };
            }
        }

        public void Hide(bool skipFade = false, Action fadeCallback = null)
        {
            if (skipFade)
            {
                _canvasGroup.alpha = 0f;
                OnHide();
                gameObject.SetActive(false);
            }
            else
            {
                var tween = _canvasGroup.DOFade(0f, fadeDuration);
                tween.onComplete += () =>
                {
                    OnHide();
                    gameObject.SetActive(false);
                    fadeCallback?.Invoke();
                };
            }
        }

        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }
    }
}