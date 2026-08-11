using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NavyGame.Runtime
{
    public class TargetSystem
    {
        public TargetSystem(TurretViewContainer container, TargetContainer targetContainer, float range)
        {
            _container = container;
            _targetContainer = targetContainer;
            _range = range;
        }

        public void Tick(float delta)
        {
            if (_targetContainer.Targets.Any())
            {
                foreach (Transform target in _targetContainer.Targets)
                {
                    CheckAddTarget(target);
                }
            }

            _targets.RemoveWhere(CheckRemoveTarget);
        }

        /// <summary>
        ///     最も近いターゲットを取得する。ターゲットが存在しない場合はnullを返す。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool TryGetClosestTarget(out Transform target)
        {
            if (_targets.Count == 0)
            {
                target = null;
                return false;
            }

            target = null;
            float minDistanceSqr = float.MaxValue;
            Vector3 selfPos = _container.Pivot.position;
            Vector3 muzzlePos = _container.Muzzle.position;

            foreach (Transform t in _targets)
            {
                if (t == null) { continue; }

                Vector3 dir = t.position - selfPos;
                float distanceSqr = dir.sqrMagnitude;
                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    target = t;
                }
            }

            return target != null;
        }

        private readonly TurretViewContainer _container;
        private readonly TargetContainer _targetContainer;
        private readonly HashSet<Transform> _targets = new();

        private readonly float _range;

        private void CheckAddTarget(Transform target)
        {
            if (target == null) { return; }
            if (_targets.Contains(target)) { return; }

            if (Check(target))
            {
                _targets.Add(target);
            }
        }

        private bool CheckRemoveTarget(Transform target)
        {
            if (target == null) { return false; }
            if (!_targets.Contains(target)) { return false; }

            return !Check(target);
        }

        private bool Check(Transform target)
        {
            float rangeSqr = _range * _range;
            Vector3 selfPos = _container.Pivot.position;
            Vector3 targetPos = target.position;
            Vector3 dir = targetPos - selfPos;

            Vector3 muzzlePos = _container.Muzzle.position;

            if (Physics.Raycast(muzzlePos, target.position - muzzlePos, out RaycastHit hit, _range))
            {
                if (hit.transform != target) { return false; }
            }

            return dir.sqrMagnitude > rangeSqr;
        }
    }
}
