using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField] private TowerCreator towerCreator;
        
        public override void InstallBindings()
        {
            Container.Bind<TowerCreator>().FromInstance(towerCreator).AsSingle();
        }
    }
}