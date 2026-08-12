using Unity.Entities;
using UnityEngine;

namespace NavyGame.Runtime
{
    public class TurretShooter
    {
        public TurretShooter(TurretViewContainer container, TurretStatus status)
        {
            if (container == null) { throw new System.ArgumentNullException(nameof(container)); }
            if (status == null) { throw new System.ArgumentNullException(nameof(status)); }
            if (status.FireRate <= 0f) { throw new System.ArgumentOutOfRangeException(nameof(status.FireRate), "Fire rate must be greater than zero."); }
            _container = container;
            _status = status;
            _timeSinceLastShot = 0f;

            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        public void Tick(float delta)
        {
            _timeSinceLastShot += delta;
            if (_timeSinceLastShot >= _status.FireRate)
            {
                Fire();
                _timeSinceLastShot = 0f;
            }
        }

        private readonly TurretViewContainer _container;
        private readonly TurretStatus _status;
        private readonly EntityManager _entityManager;
        private float _timeSinceLastShot;

        private void Fire()
        {
            Transform muzzle = _container.Muzzle;

            Vector2 spreadPoint = Random.insideUnitCircle * _status.MaxSpreadAngle;
            Quaternion spreadRotation = muzzle.rotation * Quaternion.Euler(spreadPoint.y, spreadPoint.x, 0);

            Entity entity = _entityManager.CreateEntity();
            BulletSpawnRequestComponent bulletSpawnRequest = new BulletSpawnRequestComponent
            {
                Position = muzzle.position,
                Rotation = spreadRotation,
                Index = _container.BulletIndex
            };
            _entityManager.AddComponentData(entity, bulletSpawnRequest);

            _container.InvokeOnShot();
        }
    }
}
