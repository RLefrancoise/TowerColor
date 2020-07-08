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

        public void AddBrick(Brick brick)
        {
            bricks.Add(brick);
        }

        public void EnablePhysics(bool enable)
        {
            foreach (var brick in bricks)
            {
                brick.PhysicsEnabled = enable;
            }
        }
    }
}