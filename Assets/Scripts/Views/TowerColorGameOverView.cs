using Framework.Views.Default;
using UnityEngine;

namespace TowerColor.Views
{
    public class TowerColorGameOverView : DefaultGameOverView
    {
        [SerializeField] private Animator levelFailedAnimation;

        protected override void OnShow()
        {
            base.OnShow();
            levelFailedAnimation.Play("LevelFailed");
        }
    }
}