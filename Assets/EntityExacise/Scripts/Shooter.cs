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
                Debug.Log("fire");

                float3 pos = transform.position;
                var request = _entityManager.CreateEntity();
                _entityManager.AddComponentData(request,
                    new BulletSpawnRequest(_bulletIndex, pos, transform.forward));
            }
        }
    }
}