using Cinemachine;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField] private TowerSpawner towerSpawner;
        [SerializeField] private CinemachineVirtualCamera playerCamera;
        [SerializeField] private TouchSurface touchSurface;
        
        public override void InstallBindings()
        {
            Container.Bind<TowerSpawner>().FromInstance(towerSpawner).AsSingle();
            Container.Bind<CinemachineVirtualCamera>().FromInstance(playerCamera).AsSingle();
            Container.Bind<TouchSurface>().FromInstance(touchSurface).AsSingle();
        }
    }
}