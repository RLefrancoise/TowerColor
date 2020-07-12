using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class BrickDestroyEffectInstaller : MonoInstaller<BrickDestroyEffectInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ParticleSystemRenderer>().FromComponentOnRoot();
        }
    }
}