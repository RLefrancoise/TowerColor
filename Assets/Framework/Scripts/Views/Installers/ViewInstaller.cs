using UnityEngine;
using Zenject;

namespace Framework.Views.Installers
{
    public class ViewInstaller : MonoInstaller<ViewInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<CanvasGroup>().FromComponentOnRoot();
        }
    }
}