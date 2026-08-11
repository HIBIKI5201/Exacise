using System;
using UnityEngine;

namespace NavyGame.Runtime
{
    public class InGameBoostrap : MonoBehaviour
    {
        [SerializeField]
        private ShipViewContainer _shipViewContainer;
        [SerializeField]
        private TargetContainer _targetContainer;

        private TurretLifeCycle[] _turretLifeCycle;

        public void Awake()
        {
            ShipInitializer.Init(_shipViewContainer, _targetContainer, out ShipInitializer.Result result);
            _turretLifeCycle = result.Turrets;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            foreach (TurretLifeCycle turret in _turretLifeCycle)
            {
                turret.Tick(deltaTime);
            }
        }
    }
}
