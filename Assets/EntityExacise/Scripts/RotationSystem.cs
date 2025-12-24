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
            var job = new RotationJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            };

            // 並列スケジューリング
            job.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct RotationJob : IJobEntity
        {
            public float DeltaTime;

            void Execute(in RotationSpeed speed, ref LocalTransform transform)
            {
                float degrees = speed.DegreesPerSecond * DeltaTime;
                float radians = math.radians(degrees);

                transform = transform.RotateY(radians);
            }
        }
    }
}