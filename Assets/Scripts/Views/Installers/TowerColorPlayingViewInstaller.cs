using Framework.Views.Installers;
using UnityEngine;
using Zenject;

namespace TowerColor.Views.Installers
{
    public class TowerColorPlayingViewInstaller : ViewInstaller
    {
        [SerializeField] private GameObject ballSpawnerPrefab;
        [SerializeField] private TouchSurface touchSurface;

        private Camera _playerCamera;
        
        [Inject]
        public void Construct(Camera playerCamera)
        {
            _playerCamera = playerCamera;
        }
        
        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.Bind<BallSpawner>().FromComponentInNewPrefab(ballSpawnerPrefab).AsSingle().OnInstantiated<BallSpawner>(
                (ctx, spawner) =>
                {
                    spawner.transform.SetParent(_playerCamera.transform, true);
                    spawner.transform.localScale = Vector3.one;
                });
            Container.Bind<TouchSurface>().FromInstance(touchSurface).AsSingle();
        }
    }
}