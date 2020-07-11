using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UniRx.Async;

namespace TowerColor
{
    public static class TweenExtensions
    {
        public static UniTask ToUniTask<T1, T2, TOptions>(this TweenerCore<T1, T2, TOptions> tween) where TOptions : struct, IPlugOptions
        {
            var completed = false;
            tween.onComplete += () => completed = true;
            tween.onKill += () => completed = true;
            return UniTask.WaitUntil(() => completed);
        }

        public static UniTask ToUniTask(this Tweener tweener)
        {
            var completed = false;
            tweener.onComplete += () => completed = true;
            tweener.onKill += () => completed = true;
            return UniTask.WaitUntil(() => completed);
        }
    }
}