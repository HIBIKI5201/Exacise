using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace EntityExacise
{
    public static class SimpleVisualEntityFactory
    {
        public static Entity Create(
            EntityManager em,
            Mesh mesh,
            Material material,
            float3 position,
            float degreesPerSecond)
        {
            Entity entity = em.CreateEntity();

            // Transform
            em.AddComponentData(entity, LocalTransform.FromPosition(position));

            // 回転速度（不変）
            em.AddComponentData(entity, new RotationSpeed(degreesPerSecond));

            // 描画設定
            var renderDesc = new RenderMeshDescription
            {
                FilterSettings = RenderFilterSettings.Default,
            };

            var renderArray = new RenderMeshArray(
                new[] { material },
                new[] { mesh }
            );

            RenderMeshUtility.AddComponents(
                entity,
                em,
                renderDesc,
                renderArray,
                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0)
            );

            return entity;
        }
    }
}