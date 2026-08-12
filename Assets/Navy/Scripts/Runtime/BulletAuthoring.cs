using Unity.Entities;
using UnityEngine;

namespace NavyGame.Runtime
{
    public class BulletAuthoring : MonoBehaviour
    {
        public class BulletBaker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                // Transformの変更を行うため TransformUsageFlags.Dynamic を指定
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new BulletComponent
                {
                    Speed = authoring._speed,
                    LifeTime = authoring._lifeTime,
                    CurrentTime = 0f
                });
            }
        }

        [SerializeField]
        private float _speed = 100f;
        [SerializeField]
        private float _lifeTime = 2f;
    }
}
