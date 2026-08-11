using System;
using UnityEngine;

namespace NavyGame.Runtime
{
    /// <summary>
    ///     タレットの回転を制御するクラス。
    /// </summary>
    public class TurretRotater
    {
        public TurretRotater(TurretViewContainer container)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            _container = container;
        }

        public void Look(Vector3 target)
        {
            Transform pivot = _container.Pivot;
            Transform h = _container.H;
            Transform v = _container.V;

            Vector3 targetDir = target - pivot.position;
            Vector3 localTargetDir = pivot.InverseTransformDirection(targetDir);
            if (localTargetDir.sqrMagnitude < 0.001f) { return; }

            Quaternion localTargetRot = Quaternion.LookRotation(localTargetDir);
            Vector3 euler = localTargetRot.eulerAngles;

            h.localRotation = Quaternion.Euler(0f, euler.y, 0f);
            v.localRotation = Quaternion.Euler(euler.x, 0f, 0f);
        }

        private readonly TurretViewContainer _container;
    }
}
