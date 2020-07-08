using UnityEngine;
using Zenject;

namespace TowerColor
{
    [CreateAssetMenu(fileName = "GameDataInstaller", menuName = "Installers/GameDataInstaller")]
    public class GameDataInstaller : ScriptableObjectInstaller<GameDataInstaller>
    {
        [SerializeField] private GameData gameData;
        
        public override void InstallBindings()
        {
            Container.Bind<GameData>().FromInstance(gameData).AsSingle();
        }
    }
}
