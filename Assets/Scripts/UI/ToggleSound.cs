using Zenject;

namespace TowerColor.UI
{
    public class ToggleSound : OptionToggle
    {
        private ISoundPlayer _soundPlayer;

        [Inject]
        public void Construct(ISoundPlayer soundPlayer)
        {
            _soundPlayer = soundPlayer;
        }

        protected override void OnOptionsLoaded()
        {
            toggle.isOn = OptionsManager.Options.Sound;
            _soundPlayer.SoundEnabled = OptionsManager.Options.Sound;
        }

        protected override void SetValue(bool isOn)
        {
            OptionsManager.Options.Sound = isOn;
            _soundPlayer.SoundEnabled = isOn;
        }
    }
}