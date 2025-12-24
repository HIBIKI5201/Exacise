using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace EntityExacise
{
    public class SubSceneMultiRotateAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float _degreesPerSecond = 60f; // 毎秒60度回転（元のコードは毎フレーム1度）
        [SerializeField, Min(1)]
        private int _count = 1;
        [SerializeField]
        private Vector3 _diff = Vector3.right;

        class Baker : Baker<SubSceneMultiRotateAuthoring>
        {
            /// <summary> MonoBehaviourをAuthoringクラスに変換する </summary>
            /// <param name="src"></param>
            public override void Bake(SubSceneMultiRotateAuthoring src)
            {
                // この Entity が「描画済み Prefab」になる
                Entity prefab = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(prefab, new RotationSpeed(src._degreesPerSecond));

                Entity spawner = CreateAdditionalEntity(TransformUsageFlags.None, entityName:"Spawner");
                AddComponent(spawner, new SubSceneMultiRotateAuthoringTag(
                    src._count,
                    prefab,
                    src.transform.position,
                    src._diff
                ));
            }
        }


    }

    public struct SubSceneMultiRotateAuthoringTag : IComponentData
    {
        public SubSceneMultiRotateAuthoringTag(
            int count,
            Entity prefab,
            float3 basePos,
            float3 diff)
        {
            Count = count;
            Prefab = prefab;
            BasePosition = basePos;
            Diff = diff;
        }

        public readonly int Count;
        public readonly Entity Prefab;
        public readonly float3 BasePosition;
        public readonly float3 Diff;
    }

    [BurstCompile]
    public partial struct SpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton =
                SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            var job = new SpawnJob
            {
                ECB = ecbSingleton
                    .CreateCommandBuffer(state.WorldUnmanaged)
                    .AsParallelWriter()
            };

            job.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct SpawnJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;

        void Execute(
            [EntityIndexInQuery] int index,
            Entity entity,
            in SubSceneMultiRotateAuthoringTag spawner)
        {
            for (int i = 0; i < spawner.Count; i++)
            {
                Entity e = ECB.Instantiate(index, spawner.Prefab);

                ECB.SetComponent(
                    index,
                    e,
                    LocalTransform.FromPosition(
                        spawner.BasePosition + i * spawner.Diff));
            }

            ECB.DestroyEntity(index, entity);
        }
    }

}