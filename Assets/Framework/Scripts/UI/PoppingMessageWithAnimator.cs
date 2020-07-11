using Framework.UI.Installers;
using UnityEngine;
using Zenject;

namespace Framework.UI
{
    /// <summary>
    /// Popping message with animator
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PoppingMessageWithAnimatorInstaller))]
    public class PoppingMessageWithAnimator : PoppingMessageBase
    {
        private Animator _animator;

        [Inject]
        public void Construct(Animator animator)
        {
            _animator = animator;
        }

        protected virtual void Update()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                TriggerPopOver();
                if(AutoDestroy) Destroy(gameObject);
            }
        }
    }
}