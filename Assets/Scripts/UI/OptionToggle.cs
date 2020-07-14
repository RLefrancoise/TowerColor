using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TowerColor.UI
{
    [RequireComponent(typeof(Toggle))]
    public abstract class OptionToggle : MonoBehaviour
    {
        [SerializeField] protected Toggle toggle;
        
        protected IOptionsManager OptionsManager;
        
        [Inject]
        public void Construct(IOptionsManager optionsManager)
        {
            OptionsManager = optionsManager;
            
            OptionsManager.OptionsLoaded += OnOptionsLoaded;
            
            //If no option yet, set default value according to toggle, and save
            OptionsManager.Ready += ListenReady;
            
        }

        protected virtual void Start()
        {
            toggle.onValueChanged.AddListener(ListenToggle);
        }

        protected void OnDestroy()
        {
            OptionsManager.OptionsLoaded -= OnOptionsLoaded;
            OptionsManager.Ready -= ListenReady;
            
        }

        protected abstract void OnOptionsLoaded();

        protected abstract void SetValue(bool isOn);

        private void ListenToggle(bool isOn)
        {
            SetValue(isOn);
            if (!OptionsManager.SaveOptions())
            {
                Debug.LogError("Failed to save options");
            }
        }

        private void ListenReady()
        {
            //If no option yet, set default value according to toggle, and save
            if (!OptionsManager.HasSavedOptions)
            {
                ListenToggle(toggle.isOn);
            }
        }
    }
}