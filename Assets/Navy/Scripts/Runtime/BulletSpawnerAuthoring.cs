using Unity.Entities;
using UnityEngine;

namespace NavyGame.Runtime
{
    public class BulletSpawnerAuthoring : MonoBehaviour
    {
        [SerializeField, Tooltip("生成するPrefab")]
        private BulletAuthoring[] _prefabToSpawn;

        class Bakery : Baker<BulletSpawnerAuthoring>
        {
            public override void Bake(BulletSpawnerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent<BulletCatalogComponent>(entity);

                DynamicBuffer<BulletPrefabElementData> buffer = AddBuffer<BulletPrefabElementData>(entity);

                foreach (var prefab in authoring._prefabToSpawn)
                {
                    buffer.Add(new BulletPrefabElementData
                    {
                        Prefab = GetEntity(prefab, TransformUsageFlags.Dynamic)
                    });
                }
            }
        }
    }
}
