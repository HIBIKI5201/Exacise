using System;
using System.Collections.Generic;
using UnityEngine;

namespace NavyGame.Runtime
{
    public class InGameBoostrap : MonoBehaviour
    {
        [SerializeField]
        private ShipViewContainer _shipViewContainer;
        [SerializeField]
        private TargetContainer _targetContainer;
        [SerializeField]
        private InputBuffer _inputBuffer;

        private readonly LinkedList<ITickable> _lifeCycle = new();

        public void Awake()
        {
            ShipInitializer.Init(
                _shipViewContainer, 
                _targetContainer, 
                _inputBuffer, 
                out ShipInitializer.Result result);
            
            _lifeCycle.AddLast(result.Ship);
            Array.ForEach(result.Turrets, turret => _lifeCycle.AddLast(turret));
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            foreach (ITickable turret in _lifeCycle)
            {
                turret.Tick(deltaTime);
            }
        }
    }
}
