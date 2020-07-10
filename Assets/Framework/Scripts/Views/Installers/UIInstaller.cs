using UnityEngine;
using Zenject;

namespace Framework.Views.Installers
{
    public class UIInstaller : MonoInstaller<UIInstaller>
    {
        public GameObject viewManagerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<ViewManager>().FromComponentInNewPrefab(viewManagerPrefab).AsSingle().NonLazy();
        }
    }
}