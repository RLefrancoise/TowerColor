using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TowerColor.UI
{
    /// <summary>
    /// Base class for options toggle
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public abstract class OptionToggle : MonoBehaviour
    {
        /// <summary>
        /// Toggle
        /// </summary>
        [SerializeField] protected Toggle toggle;
        
        /// <summary>
        /// Options manager
        /// </summary>
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
        
        /// <summary>
        /// When options are loaded 
        /// </summary>
        protected abstract void OnOptionsLoaded();

        /// <summary>
        /// Set option value according to toggle
        /// </summary>
        /// <param name="isOn"></param>
        protected abstract void SetValue(bool isOn);

        /// <summary>
        /// Listen toggle change
        /// </summary>
        /// <param name="isOn"></param>
        private void ListenToggle(bool isOn)
        {
            SetValue(isOn);
            if (!OptionsManager.SaveOptions())
            {
                Debug.LogError("Failed to save options");
            }
        }

        /// <summary>
        /// Listen options manager ready
        /// </summary>
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