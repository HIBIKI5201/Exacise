using UnityEngine;

namespace NavyGame.Runtime
{
    public class TurretLifeCycle : ITickable
    {
        public TurretLifeCycle(TurretRotater rotater, TurretShooter shooter, TargetSystem targetSystem)
        {
            _rotater = rotater;
            _shooter = shooter;
            _targetSystem = targetSystem;
        }

        public void Tick(float deltaTime)
        {
            _targetSystem.Tick(deltaTime);

            if (_targetSystem.TryGetClosestTarget(out Transform target))
            {
                _rotater.Look(target.position);
                _shooter.Tick(deltaTime);
            }
        }

        private readonly TurretRotater _rotater;
        private readonly TurretShooter _shooter;
        private readonly TargetSystem _targetSystem;
    }
}
