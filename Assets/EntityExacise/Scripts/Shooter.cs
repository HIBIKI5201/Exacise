using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EntityExacise
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private int _bulletIndex = 0;
        EntityManager _entityManager;

        void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float3 pos = transform.position + transform.forward * 1.0f;

                var request = _entityManager.CreateEntity();
                _entityManager.AddComponentData(request, new BulletSpawnRequest
                {
                    PrefabIndex = _bulletIndex,
                    Position = pos,
                    Forward = transform.forward
                });
            }
        }
    }
}