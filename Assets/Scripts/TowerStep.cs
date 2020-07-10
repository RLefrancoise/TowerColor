using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace TowerColor
{
    public class TowerStep : MonoBehaviour
    {
        [ShowNonSerializedField] private Vector3 _startPosition;
        
        [SerializeField] private List<Brick> bricks;

        public ReadOnlyCollection<Brick> Bricks => bricks.AsReadOnly();

        public float Height => bricks[0].Height;
        
        [ShowNativeProperty] public bool IsActivated { get; private set; }

        [ShowNativeProperty] public bool IsFullyDestroyed { get; private set; }
        
        public event Action FullyDestroyed;

        private void Start()
        {
            _startPosition = transform.position;
            
            foreach (var brick in bricks)
            {
                brick.Destroyed += OnBrickDestroyed;
            }
        }

        private void Update()
        {
            if(IsFullyDestroyed) return;

            var allBricksMoved = bricks.Count == 0 || bricks.All(b => !b.IsStillInPlace);
            if (allBricksMoved)
            {
                Debug.LogFormat("Step {0} fully destroyed", name);
                IsFullyDestroyed = true;
                FullyDestroyed?.Invoke();
            }
        }
        
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
        
        private void OnBrickDestroyed(Brick brick)
        {
            bricks.Remove(brick);
        }
    }
}