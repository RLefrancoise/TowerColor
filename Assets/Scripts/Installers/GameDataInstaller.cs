using Framework.Scripts.Data;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    [CreateAssetMenu(fileName = "GameDataInstaller", menuName = "Installers/GameDataInstaller")]
    public class GameDataInstaller : ScriptableObjectInstaller<GameDataInstaller>
    {
        /// <summary>
        /// Game data
        /// </summary>
        [SerializeField] private GameData gameData;
        
        /// <summary>
        /// Player save data
        /// </summary>
        private SaveData _saveData;
        
        public override void InstallBindings()
        {
            Container.Bind<GameData>().FromInstance(gameData).AsSingle().NonLazy();
            Container.Bind<ISaveData>().FromInstance(new SaveData()).AsSingle().NonLazy();
        }
    }
}
