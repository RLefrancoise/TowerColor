using Framework.Scripts.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Framework.Game
{
    public class LevelManager : MonoBehaviour
    {
        private ISaveData _saveData;
        
        public int CurrentLevel
        {
            get => _saveData.CurrentLevel;
            set
            {
                _saveData.CurrentLevel = value;
                //Reload the scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        [Inject]
        public void Construct(ISaveData saveData)
        {
            _saveData = saveData;
        }
    }
}