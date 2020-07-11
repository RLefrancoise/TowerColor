using System;
using UnityEngine;

namespace Framework.UI
{
    /// <summary>
    /// Base class for popping message
    /// </summary>
    public abstract class PoppingMessageBase : MonoBehaviour, IPoppingMessage
    {
        [SerializeField] private bool autoDestroy = true;
        
        public bool AutoDestroy
        {
            get => autoDestroy;
            set => autoDestroy = value;
        }

        public void AttachTo(Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
        }

        public event Action PopOver;

        protected void TriggerPopOver()
        {
            PopOver?.Invoke();
        }
    }
}