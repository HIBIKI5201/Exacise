using UnityEngine;

namespace NavyGame.Runtime
{
    public class TurretLifeCycle
    {
        public TurretLifeCycle(TurretRotater rotater, TurretShooter shooter, TargetSystem targetSystem)
        {
            _rotater = rotater;
            _shooter = shooter;
            _targetSystem = targetSystem;
        }

        public void Tick(float delta)
        {
            _targetSystem.Tick(delta);

            if (_targetSystem.TryGetClosestTarget(out Transform target))
            {
                _rotater.Look(target.position);
                _shooter.Tick(delta);
            }
        }

        private readonly TurretRotater _rotater;
        private readonly TurretShooter _shooter;
        private readonly TargetSystem _targetSystem;
    }
}
