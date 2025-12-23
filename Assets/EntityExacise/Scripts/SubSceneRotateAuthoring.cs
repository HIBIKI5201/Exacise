using Unity.Entities;
using UnityEngine;

namespace EntityExacise
{
    /// <summary> ComponentData </summary>
    public struct RotationSpeed : IComponentData
    {
        public RotationSpeed(float degreesPerSecond)
        {
            DegreesPerSecond = degreesPerSecond;
        }

        public readonly float DegreesPerSecond;
    }

    /// <summary> AuthoringClass </summary>
    public class SubSceneRotateAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float _degreesPerSecond = 60f; // 毎秒60度回転（元のコードは毎フレーム1度）

        class Baker : Baker<SubSceneRotateAuthoring>
        {
            /// <summary> MonoBehaviourをAuthoringクラスに変換する </summary>
            /// <param name="src"></param>
            public override void Bake(SubSceneRotateAuthoring src)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new RotationSpeed(src._degreesPerSecond));
            }
        }
    }
}