using Cinemachine;
using Framework.Game.Installers;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class GameInstaller : GameInstaller<GameManager>
    {
        [SerializeField] private SoundPlayer soundPlayer;
        [SerializeField] private TowerCreator towerCreator;
        [SerializeField] private TowerSpawner towerSpawner;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private CinemachineVirtualCamera playerGameCamera;
        [SerializeField] private CinemachineVirtualCamera lookAroundTowerCamera;

        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.BindInterfacesAndSelfTo<SoundPlayer>().FromInstance(soundPlayer).AsSingle();
            Container.BindInterfacesAndSelfTo<HapticManager>().FromInstance(new HapticManager()).AsSingle();

            Container.Bind<TowerCreator>().FromInstance(towerCreator).AsSingle();
            Container.Bind<TowerSpawner>().FromInstance(towerSpawner).AsSingle();
            Container.Bind<Camera>().FromInstance(playerCamera).AsSingle();
            Container.Bind<CinemachineVirtualCamera>().WithId("GameCamera")
                .FromInstance(playerGameCamera).AsCached();
            Container.Bind<CinemachineVirtualCamera>().WithId("LookAroundTowerCamera")
                .FromInstance(lookAroundTowerCamera).AsCached();
            
            //Ball bonus
            Container.BindFactory<BallBonus, BallBonus.Factory>().FromFactory<BallBonusFactory>();
            
            //Brick destroy effect
            Container.BindFactory<GameObject, BrickDestroyEffect, BrickDestroyEffect.Factory>()
                .FromFactory<PrefabFactory<BrickDestroyEffect>>().NonLazy();
        }
    }
}