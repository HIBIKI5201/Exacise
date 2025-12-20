using System;
using UnityEngine;

namespace AbeShootingGame
{
    public class BulletManager : MonoBehaviour
    {
        public event Action<BulletManager> OnHitted;
        public float CollisionRadius => _collisionRadius;
        public void Fire(Vector3 direction)
        {
            _direction = direction;
        }

        public void Hit()
        {
            _direction = Vector3.zero;
            OnHitted?.Invoke(this);
        }

        [SerializeField]
        private float _speed = 5;
        [SerializeField]
        private float _collisionRadius = 1;

        private Vector3 _direction;

        private void Update()
        {
            transform.position += _direction * Time.deltaTime * _speed;
        }
    }
}
