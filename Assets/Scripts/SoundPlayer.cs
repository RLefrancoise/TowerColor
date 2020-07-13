using UnityEngine;

namespace TowerColor
{
    /// <summary>
    /// Sound player
    /// </summary>
    public class SoundPlayer : MonoBehaviour, ISoundPlayer
    {
        public bool SoundEnabled { get; set; }
        
        public void PlaySound(AudioSource sound)
        {
            if (!SoundEnabled) return;
            sound.Play();
        }
    }
}