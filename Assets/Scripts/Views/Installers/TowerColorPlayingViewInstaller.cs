using Framework.UI;
using Framework.Views.Installers;
using UnityEngine;
using Zenject;

namespace TowerColor.Views.Installers
{
    public class TowerColorPlayingViewInstaller : ViewInstaller
    {
        [SerializeField] private Transform colorChangeMessageAnchor;
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
            
            //Ball spawner
            Container.BindInterfacesAndSelfTo<BallSpawner>().FromComponentInNewPrefab(ballSpawnerPrefab).AsSingle().OnInstantiated<BallSpawner>(
                (ctx, spawner) =>
                {
                    spawner.transform.SetParent(_playerCamera.transform, true);
                    spawner.transform.localScale = Vector3.one;
                });
            
            //Touch surface
            Container.BindInterfacesAndSelfTo<TouchSurface>().FromInstance(touchSurface).AsSingle();

            //Popping message factory
            Container.BindFactory<Object, IPoppingMessage, PoppingMessageFactory>()
                .FromFactory<PrefabFactory<IPoppingMessage>>();
            
            //Color change message anchor
            Container.Bind<Transform>().WithId("ColorChangeMessageAnchor").FromInstance(colorChangeMessageAnchor);
        }
    }
}