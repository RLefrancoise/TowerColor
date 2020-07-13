using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// Interface for sound player
    /// </summary>
    public interface ISoundPlayer
    {
        /// <summary>
        /// Is sound enabled ?
        /// </summary>
        bool SoundEnabled { get; set; }
        
        /// <summary>
        /// Play the given sound
        /// </summary>
        /// <param name="sound">Sound to play</param>
        void PlaySound(AudioSource sound);
    }
}