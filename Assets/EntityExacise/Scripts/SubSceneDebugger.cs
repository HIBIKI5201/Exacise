using UnityEngine;
using Unity.Scenes;
namespace EntityExacise
{
    [RequireComponent(typeof(SubScene))]
    public class SubSceneDebugger : MonoBehaviour
    {
        private SubScene _subScene;

        private void Awake()
        {
            _subScene = GetComponent<SubScene>();
        }

        private void Start()
        {
            EntityId id = _subScene.GetEntityId();
            Debug.Log($"entity id: {id}");
        }
    }
}
