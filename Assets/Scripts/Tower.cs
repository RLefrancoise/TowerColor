using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace TowerColor
{
    public class Tower : MonoBehaviour
    {
        private List<TowerStep> _steps;

        public ReadOnlyCollection<TowerStep> Steps => _steps.AsReadOnly();

        private void Awake()
        {
            _steps = new List<TowerStep>();
        }

        public void AddStep(TowerStep step)
        {
            _steps.Add(step);
        }

        public void EnablePhysics(bool enable)
        {
            foreach (var step in _steps)
            {
                step.EnablePhysics(enable);
            }
        }
    }
}