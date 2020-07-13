using UnityEngine;
using Zenject;

namespace TowerColor
{
    public class BrickInstaller : MonoInstaller<BrickInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<AudioSource>().FromInstance(GetComponentInParent<TowerStep>().GetComponent<AudioSource>());
        }
    }
}