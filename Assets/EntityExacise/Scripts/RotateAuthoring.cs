using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Rendering;
using UnityEngine;

namespace EntityExacise
{
    /// <summary> ComponentData </summary>
    public struct RotationSpeed : IComponentData
    {
        public float DegreesPerSecond;
        // public Mesh Mesh;
        // public Material Material;
    }

    /// <summary> AuthoringClass </summary>
    public class RotateAuthoring : MonoBehaviour
    {
        public float DegreesPerSecond = 60f; // 毎秒60度回転（元のコードは毎フレーム1度）
        public Mesh Mesh;              // Inspector で設定
        public Material Material;       // Inspector で設定

        private void OnValidate()
        {
            if (TryGetComponent(out MeshRenderer renderer)) { Material = renderer.sharedMaterial; }
            if (TryGetComponent(out MeshFilter filter)) { Mesh = filter.sharedMesh; }
        }

        class Baker : Baker<RotateAuthoring>
        {
            /// <summary> MonoBehaviourをAuthoringクラスに変換する </summary>
            /// <param name="src"></param>
            public override void Bake(RotateAuthoring src)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new RotationSpeed() { DegreesPerSecond = src.DegreesPerSecond });
                // レンダリング情報を明示的に追加
                var renderMeshDescription = new RenderMeshDescription
                {
                    FilterSettings = RenderFilterSettings.Default,
                    LightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off,
                };
                var renderMeshArray = new RenderMeshArray(
                    new[] { src.Material },
                    new[] { src.Mesh }
                );
                RenderMeshUtility.AddComponents( //あくまで描画専用の関数
                    entity,
                    new EntityManager(),
                    renderMeshDescription,
                    renderMeshArray,
                    MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0)
                );
            }
        }
    }
}