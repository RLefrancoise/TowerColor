using Zenject;

namespace TowerColor.UI
{
    /// <summary>
    /// Toggle for haptics option
    /// </summary>
    public class ToggleHaptics : OptionToggle
    {
        /// <summary>
        /// Haptic manager
        /// </summary>
        private IHapticManager _hapticManager;

        [Inject]
        public void Construct(IHapticManager hapticManager)
        {
            _hapticManager = hapticManager;
        }

        protected override void OnOptionsLoaded()
        {
            toggle.isOn = OptionsManager.Options.Vibration;
            _hapticManager.VibrationEnabled = OptionsManager.Options.Vibration;
        }

        protected override void SetValue(bool isOn)
        {
            OptionsManager.Options.Vibration = isOn;
            _hapticManager.VibrationEnabled = isOn;
        }
    }
}