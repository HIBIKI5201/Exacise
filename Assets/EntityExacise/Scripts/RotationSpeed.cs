using Unity.Entities;
using UnityEngine;

namespace EntityExacise
{
    /// <summary> ComponentData </summary>
    public struct RotationSpeed : IComponentData
    {
        public RotationSpeed(float degreesPerSecond)
        {
            DegreesPerSecond = degreesPerSecond;
        }

        public readonly float DegreesPerSecond;
    }
}