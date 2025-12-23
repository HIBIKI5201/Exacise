using System;
using Unity.Entities;
using UnityEngine;

namespace EntityExacise
{
    public class FactoryRotateCreater : MonoBehaviour
    {
        [SerializeField]
        private Material _mat;
        [SerializeField]
        private Mesh _mesh;
        [SerializeField]
        private EntityData[] _datas;

        [Serializable]
        private struct EntityData
        {
            public Vector3 Position => _position;
            public float RotationSpeed => _rotationSpeed;

            [SerializeField]
            private Vector3 _position;
            [SerializeField]
            private float _rotationSpeed;
        }

        private void Start()
        {
            EntityManager em =
                World.DefaultGameObjectInjectionWorld.EntityManager;

            foreach (EntityData data in _datas)
            {
                SimpleVisualEntityFactory.Create(
                    em,
                    _mesh,
                    _mat,
                    data.Position,
                    data.RotationSpeed
                    );
            }
        }
    }
}
