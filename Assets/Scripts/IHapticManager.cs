namespace TowerColor
{
    /// <summary>
    /// Interface for haptic manager
    /// </summary>
    public interface IHapticManager
    {
        /// <summary>
        /// Is vibration enabled ?
        /// </summary>
        bool VibrationEnabled { get; set; }
        
        /// <summary>
        /// Vibrate for 1 second
        /// </summary>
        void Vibrate();
    }
}