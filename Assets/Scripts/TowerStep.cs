using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace TowerColor
{
    public class TowerStep : MonoBehaviour
    {
        [SerializeField] private List<Brick> bricks;

        public ReadOnlyCollection<Brick> Bricks => bricks.AsReadOnly();

        public float Height => bricks[0].Height;
        
        public bool IsActivated { get; private set; }

        public void EnablePhysics(bool enable)
        {
            foreach (var brick in bricks)
            {
                brick.PhysicsEnabled = enable;
            }
        }

        public void ActivateStep(bool activate)
        {
            IsActivated = activate;
            
            foreach(var brick in bricks)
                brick.SetActivated(activate);
        }
    }
}