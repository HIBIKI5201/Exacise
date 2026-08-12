using Unity.Entities;

namespace NavyGame.Runtime
{
    public struct BulletComponent : IComponentData
    {
        public float Speed;
        public float LifeTime;
        public float CurrentTime;
    }
}
