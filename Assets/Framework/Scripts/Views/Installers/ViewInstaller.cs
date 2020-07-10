using UnityEngine;
using Zenject;

namespace Framework.Views.Installers
{
    public class ViewInstaller : MonoInstaller<ViewInstaller>
    {
        [SerializeField] private CanvasGroup canvasGroup;
        
        public override void InstallBindings()
        {
            Container.Bind<CanvasGroup>().FromInstance(canvasGroup);
        }
    }
}