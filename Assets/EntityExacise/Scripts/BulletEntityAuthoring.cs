using EntityExacise;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace EntityExacise
{
    public class BulletEntityAuthoring : MonoBehaviour
    {
        [SerializeField] private float _velocity = 1;
        [SerializeField] private Vector3 _direction = Vector3.up;
        class Baker : Baker<BulletEntityAuthoring>
        {
            public override void Bake(BulletEntityAuthoring src)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new BulletVelocity(src._velocity, src._direction));
            }
        }
    }

    [BurstCompile]
    public partial struct BulletMoveSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var job = new BulletMoveJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            };

            // 並列スケジューリング
            job.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct BulletMoveJob : IJobEntity
        {
            public float DeltaTime;

            void Execute(in BulletVelocity velocity, ref LocalTransform transform)
            {
                transform.Position = transform.Position + velocity.Direction * velocity.Speed * DeltaTime;
            }
        }
    }
}
