using UnityEngine;
using Zenject;

namespace Framework.UI.Installers
{
    public class PoppingMessageWithAnimatorInstaller : MonoInstaller<PoppingMessageWithAnimatorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<Animator>().FromComponentOnRoot();
        }
    }
}