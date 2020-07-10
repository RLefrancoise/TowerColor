using Framework.Scripts.Data;
using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// The save data of the player
    /// </summary>
    public class SaveData : ISaveData
    {
        /// <summary>
        /// Current level of the player
        /// </summary>
        public int CurrentLevel
        {
            get => PlayerPrefs.HasKey("CurrentLevel") ? PlayerPrefs.GetInt("CurrentLevel") : 1;
            set => PlayerPrefs.SetInt("CurrentLevel", value);
        }
    }
}