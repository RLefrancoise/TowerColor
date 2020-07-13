using Framework.Views.Installers;
using UnityEngine;

namespace TowerColor.Views.Installers
{
    public class TowerColorMenuViewInstaller : ViewInstaller
    {
        [SerializeField] private OptionsManager optionsManager;

        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.BindInterfacesAndSelfTo<OptionsManager>().FromInstance(optionsManager).AsSingle();
        }
    }
}