using Zenject;

namespace TowerColor.UI
{
    public class ToggleHaptics : OptionToggle
    {
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