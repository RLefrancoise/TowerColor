using Framework.UI.Installers;
using TMPro;

namespace TowerColor.UI.Installers
{
    public class BallGainedMessageInstaller : PoppingMessageWithAnimatorInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind<TMP_Text>().FromComponentInChildren();
        }
    }
}