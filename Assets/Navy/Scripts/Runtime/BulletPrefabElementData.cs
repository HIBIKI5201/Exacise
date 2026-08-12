using Unity.Entities;

namespace NavyGame.Runtime
{
    public struct BulletPrefabElementData : IBufferElementData
    {
        public Entity Prefab;
    }

    public struct BulletCatalogComponent : IComponentData { }
}
