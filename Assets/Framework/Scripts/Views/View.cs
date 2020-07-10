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
        [ShowNonSerializedField] protected CanvasGroup _canvasGroup;
        [SerializeField] protected float fadeDuration = 1f;
        
        public abstract GameState State { get; }
        
        [Inject]
        public void Construct(CanvasGroup canvasGroup)
        {
            _canvasGroup = canvasGroup;
        }
        
        public void Show(bool skipFade = false, Action fadeCallback = null)
        {
            if (skipFade) _canvasGroup.alpha = 1f;
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
            if (skipFade) _canvasGroup.alpha = 0f;
            else
            {
                var tween = _canvasGroup.DOFade(0f, fadeDuration);
                tween.onComplete += () =>
                {
                    OnHide();
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