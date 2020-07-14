using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// Options manager
    /// </summary>
    public class OptionsManager : MonoBehaviour, IOptionsManager
    {
        /// <summary>
        /// Options data file path
        /// </summary>
        private string _optionsPath;
        
        public OptionsData Options { get; private set; }

        public bool HasSavedOptions => File.Exists(_optionsPath);

        public event Action OptionsLoaded;
        public event Action Ready;
        
        private void Awake()
        {
            _optionsPath = Path.Combine(Application.persistentDataPath, "options.json");
            Options = new OptionsData();
            Ready?.Invoke();
        }
        
        public bool SaveOptions()
        {
            if (Options == null)
            {
                Debug.LogError("OptionsManager:SaveOptions : Options data is null");
                return false;
            }
            
            try
            {
                var json = JsonUtility.ToJson(Options);
                File.WriteAllBytes(_optionsPath, Encoding.UTF8.GetBytes(json));
                Debug.LogFormat("OptionsManager:SaveOptions - Saved options : {0}", json);
            }
            catch (IOException e)
            {
                Debug.LogErrorFormat("OptionsManager:SaveOptions - {0}", e.StackTrace);
                return false;
            }

            return true;
        }

        public bool LoadOptions()
        {
            if (!HasSavedOptions) return false;

            try
            {
                Options = JsonUtility.FromJson<OptionsData>(File.ReadAllText(_optionsPath));
                if (Options != null)
                {
                    Debug.LogFormat("OptionsManager:LoadOptions - Options loaded : {0}", JsonUtility.ToJson(Options));
                    OptionsLoaded?.Invoke();
                }
                else
                    Debug.Log("OptionsManager:LoadOptions - Failed to deserialize options data");
            }
            catch (IOException e)
            {
                Options = null;
                Debug.LogErrorFormat("OptionsManager:LoadOptions - {0}", e.StackTrace);
            }
            
            return Options != null;
        }
    }
}