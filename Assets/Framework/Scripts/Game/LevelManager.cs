using Framework.Scripts.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Framework.Game
{
    public interface ILevelManager
    {
        int CurrentLevel { get; set; }
        void ReloadLevel();

        float GetCurveValue(AnimationCurve curve);
    }
    
    public class LevelManager : MonoBehaviour, ILevelManager
    {
        private ISaveData _saveData;
        
        public int CurrentLevel
        {
            get => _saveData.CurrentLevel;
            set
            {
                _saveData.CurrentLevel = value;
                //Reload the scene
                ReloadLevel();
            }
        }

        [Inject]
        public void Construct(ISaveData saveData)
        {
            _saveData = saveData;
        }

        public void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public float GetCurveValue(AnimationCurve curve)
        {
            return curve.Evaluate(CurrentLevel);
        }
    }
}