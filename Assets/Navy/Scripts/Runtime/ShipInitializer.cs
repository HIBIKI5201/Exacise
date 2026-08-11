using System;
using Unity.Collections;
using UnityEngine;

namespace NavyGame.Runtime
{
    public static class ShipInitializer
    {
        public static void Init(ShipViewContainer container, TargetContainer targetContainer, out Result result)
        {
            ShipStatus status = container.Status;
            TurretLifeCycle[] turretResults = new TurretLifeCycle[container.Turrets.Length];
            for (int i = 0; i < container.Turrets.Length; i++)
            {
                TurretInitializer.Init(container.Turrets[i], targetContainer, out TurretInitializer.Result rlt);
                turretResults[i] = rlt.LifeCycle;
            }

            result = new Result(turretResults);
        }
        public readonly ref struct Result
        {
            public readonly TurretLifeCycle[] Turrets;
            public Result(TurretLifeCycle[] turrets)
            {
                Turrets = turrets;
            }
        }
    }
}
