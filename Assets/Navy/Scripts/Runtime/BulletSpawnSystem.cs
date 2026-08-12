using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace NavyGame.Runtime
{
    [BurstCompile]
    public partial struct BulletSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.HasSingleton<BulletPrefabElementData>()) { return; }

            BeginSimulationEntityCommandBufferSystem.Singleton ecbSingleton =
                SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            EntityCommandBuffer ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            Entity catalogEntity = SystemAPI.GetSingletonEntity<BulletCatalogComponent>();
            DynamicBuffer<BulletPrefabElementData> buffer = SystemAPI.GetBuffer<BulletPrefabElementData>(catalogEntity);

            foreach (var (request, reqEntity) in SystemAPI.Query<RefRO<BulletSpawnRequestComponent>>().WithEntityAccess())
            {
                Entity newBullet = ecb.Instantiate(buffer[request.ValueRO.Index].Prefab);

                ecb.SetComponent(newBullet, LocalTransform.FromPositionRotation(
                    request.ValueRO.Position,
                    request.ValueRO.Rotation
                ));

                ecb.DestroyEntity(reqEntity);
            }
        }
    }
}
