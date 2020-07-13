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
        }

        protected virtual void Start()
        {
            toggle.onValueChanged.AddListener(ListenToggle);
        }

        protected void OnDestroy()
        {
            OptionsManager.OptionsLoaded -= OnOptionsLoaded;
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
    }
}