using Framework.UI;
using UnityEngine;
using Zenject;

namespace Framework.Game.Installers
{
    public class GameInstaller<TGameManager> : MonoInstaller<GameInstaller<TGameManager>> where TGameManager : GameManagerBase
    {
        [SerializeField] private TGameManager gameManager;
        [SerializeField] private LevelManager levelManager;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TGameManager>().FromInstance(gameManager).AsSingle();
            Container.BindInterfacesAndSelfTo<LevelManager>().FromInstance(levelManager).AsSingle();
            
            //Popping message factory
            Container.BindFactory<Object, IPoppingMessage, PoppingMessageFactory>()
                .FromFactory<PrefabFactory<IPoppingMessage>>();
        }
    }
}