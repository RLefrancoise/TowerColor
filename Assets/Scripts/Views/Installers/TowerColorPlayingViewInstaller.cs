using Framework.Views.Installers;
using UnityEngine;

namespace TowerColor.Views.Installers
{
    public class TowerColorPlayingViewInstaller : ViewInstaller
    {
        [SerializeField] private TouchSurface touchSurface;
        
        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.Bind<BallSpawner>().FromInstance(FindObjectOfType<BallSpawner>()).AsSingle();
            Container.Bind<TouchSurface>().FromInstance(touchSurface).AsSingle();
        }
    }
}