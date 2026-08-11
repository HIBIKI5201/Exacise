using PlasticGui.WorkspaceWindow.Locks;
using UnityEngine;

namespace NavyGame.Runtime
{
    [CreateAssetMenu(fileName = nameof(ShipStatus), menuName = "NavyGame/" + nameof(ShipStatus))]
    public class ShipStatus : ScriptableObject
    {
        public float Acceleration => _acceleration;
        public float MaxSpeed => _maxSpeed;
        public float TurnSpeed => _turnSpeed;

        [SerializeField, Tooltip("加速度")]
        private float _acceleration = 5f;

        [SerializeField, Tooltip("最大速度")]
        private float _maxSpeed = 10f;

        [SerializeField, Tooltip("旋回速度")]
        private float _turnSpeed = 100f;
    }
}
