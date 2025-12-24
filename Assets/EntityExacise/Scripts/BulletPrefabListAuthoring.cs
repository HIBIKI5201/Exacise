using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace EntityExacise
{
    // プレハブアセットを配列で持つ
    public class BulletPrefabListAuthoring : MonoBehaviour
    {
        public GameObject[] bulletPrefabs;

        class BulletPrefabListBaker : Baker<BulletPrefabListAuthoring>
        {
            public override void Bake(BulletPrefabListAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                // バッファを追加
                var buffer = AddBuffer<BulletPrefabElement>(entity);

                // GameObject プレハブから Entity プレハブを取得して追加
                foreach (var prefab in authoring.bulletPrefabs)
                {
                    buffer.Add(new BulletPrefabElement
                    {
                        Prefab = GetEntity(prefab, TransformUsageFlags.Dynamic)
                    });
                }
            }
        }
    }

    public struct BulletSpawnRequest : IComponentData
    {
        public int PrefabIndex;
        public float3 Position;
        public float3 Forward;
    }

    public struct BulletPrefabElement : IBufferElementData
    {
        public Entity Prefab;
    }

    public struct BulletVelocity : IComponentData
    {
        public BulletVelocity(float value, float3 dir)
        {
            Value = value;
            Direction = dir;
        }

        public float3 Value;
        public float3 Direction;
    }

    public partial struct BulletSpawnSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.TryGetSingletonBuffer<BulletPrefabElement>(out var prefabBuffer))
                return;

            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

            foreach (var (request, entity)
                in SystemAPI.Query<RefRO<BulletSpawnRequest>>()
                            .WithEntityAccess())
            {
                int index = request.ValueRO.PrefabIndex;
                if (index < 0 || index >= prefabBuffer.Length)
                    continue;

                Entity prefab = prefabBuffer[index].Prefab;
                Entity bullet = ecb.Instantiate(prefab);

                ecb.SetComponent(bullet, LocalTransform.FromPosition(
                    request.ValueRO.Position
                ));

                ecb.AddComponent(bullet, new BulletVelocity
                {
                    Value = request.ValueRO.Forward * 10f
                });

                // リクエスト消費
                ecb.DestroyEntity(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}