using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace NavyGame.Runtime
{
    [BurstCompile]
    public partial struct BulletMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            EndSimulationEntityCommandBufferSystem.Singleton ecbSingleton = 
                SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            EntityCommandBuffer.ParallelWriter ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            // Jobの平行実行
            state.Dependency = new BulletUpdateJob
            {
                DeltaTime = deltaTime,
                Ecb = ecb
            }.ScheduleParallel(state.Dependency);
        }
    }

    [BurstCompile]
    public partial struct BulletUpdateJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter Ecb;

        void Execute(
            [ChunkIndexInQuery] int chunkIndex,
            Entity entity,
            ref BulletComponent bullet,
            ref LocalTransform transform)
        {
            transform.Position += transform.Forward() * (bullet.Speed * DeltaTime);

            bullet.CurrentTime += DeltaTime;
            if (bullet.CurrentTime >= bullet.LifeTime)
            {
                Ecb.DestroyEntity(chunkIndex, entity);
            }
        }
    }
}