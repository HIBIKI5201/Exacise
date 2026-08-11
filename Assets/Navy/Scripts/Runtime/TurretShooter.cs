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
        private float _timeSinceLastShot;

        private void Fire()
        {
            Transform muzzle = _container.Muzzle;
            BulletMover bullet = _container.Bullet;

            Vector2 spreadPoint = Random.insideUnitCircle * _status.MaxSpreadAngle;
            Quaternion spreadRotation = muzzle.rotation * Quaternion.Euler(spreadPoint.y, spreadPoint.x, 0);

            BulletMover b = Object.Instantiate(bullet, muzzle.position, spreadRotation);
            b.Init();

            _container.InvokeOnShot();
        }
    }
}
