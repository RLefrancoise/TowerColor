using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class BrickInstaller : MonoInstaller<BrickInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<Renderer>().FromComponentInChildren().AsSingle();
            Container.Bind<Collider>().FromComponentInChildren().AsSingle();
        }
    }
}