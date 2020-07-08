using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace TowerColor
{
    [CreateAssetMenu(fileName = "GameData", menuName = "TowerColor/GameData")]
    public class GameData : ScriptableObject
    {
        [Serializable]
        public class BrickColorData
        {
            public Color color;
            public Material material;
        }
        
        /// <summary>
        /// Available brick colors
        /// </summary>
        [BoxGroup("Brick")]
        [Tooltip("Available brick colors")]
        public List<Material> brickColors;

        /// <summary>
        /// Available tower profiles
        /// </summary>
        [BoxGroup("Tower")]
        [Tooltip("Available tower profiles")]
        public List<TowerProfile> towerProfiles;
    }
}