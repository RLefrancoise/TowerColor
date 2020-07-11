using UnityEngine;
using Zenject;

namespace Framework.Views.Installers
{
    public class UIInstaller : MonoInstaller<UIInstaller>
    {
        public GameObject viewManagerPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ViewManager>().FromComponentInNewPrefab(viewManagerPrefab).AsSingle().NonLazy();
        }
    }
}