using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EntityExacise
{
    [BurstCompile]
    public partial struct RotationSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (speed, transform) in
                     SystemAPI.Query<RotationSpeed, RefRW<LocalTransform>>())
            {
                float degrees = speed.DegreesPerSecond * deltaTime;
                float radians = math.radians(degrees);

                transform.ValueRW =
                    transform.ValueRW.RotateY(radians);
            }
        }
    }
}