using Cinemachine;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField] private TowerSpawner towerSpawner;
        [SerializeField] private BallSpawner ballSpawner;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private CinemachineVirtualCamera playerGameCamera;
        [SerializeField] private TouchSurface touchSurface;
        
        public override void InstallBindings()
        {
            Container.Bind<TowerSpawner>().FromInstance(towerSpawner).AsSingle();
            Container.Bind<BallSpawner>().FromInstance(ballSpawner).AsSingle();
            Container.Bind<Camera>().FromInstance(playerCamera).AsSingle();
            Container.Bind<CinemachineVirtualCamera>().WithId("GameCamera").FromInstance(playerGameCamera).AsSingle();
            Container.Bind<TouchSurface>().FromInstance(touchSurface).AsSingle();
        }
    }
}