using System;
using UnityEngine;

namespace Framework.UI
{
    /// <summary>
    /// Interface for any popping message we can have in many games
    /// </summary>
    public interface IPoppingMessage
    {
        /// <summary>
        /// Message should auto destroy
        /// </summary>
        bool AutoDestroy { get; set; }

        void AttachTo(Transform parent);
        
        /// <summary>
        /// When popping is over
        /// </summary>
        event Action PopOver;
    }
}