using TMPro;
using Zenject;

namespace TowerColor
{
    public class BallBonusInstaller : MonoInstaller<BallBonusInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<CollisionEvents>().FromComponentInChildren();
            Container.Bind<TMP_Text>().FromComponentInChildren();
        }
    }
}