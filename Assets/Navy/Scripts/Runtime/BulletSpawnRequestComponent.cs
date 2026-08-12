using Unity.Entities;
using Unity.Mathematics;

namespace NavyGame.Runtime
{
    internal struct BulletSpawnRequestComponent : IComponentData
    {
        public byte Index;
        public float3 Position;
        public quaternion Rotation;
    }
}
