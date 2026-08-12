using UnityEngine;

namespace NavyGame.Runtime
{
    [CreateAssetMenu(fileName = nameof(ShipStatus), menuName = "NavyGame/" + nameof(ShipStatus))]
    public class ShipStatus : ScriptableObject
    {
        public float Acceleration => _acceleration;
        public float MaxSpeed => _maxSpeed;
        public float TurnAcceleration => _turnAcceleration;
        public float MaxTurnSpeed => _maxTurnSpeed;
        public float TurnDrag => _turnDrag;
        public float MoveDrag => _moveDrag;

        [SerializeField, Tooltip("加速度")]
        private float _acceleration = 5f;

        [SerializeField, Tooltip("最大速度")]
        private float _maxSpeed = 10f;

        [SerializeField, Tooltip("旋回加速度")]
        private float _turnAcceleration = 100f;

        [SerializeField, Tooltip("最大旋回速度")]
        private float _maxTurnSpeed = 100f;

        [SerializeField, Tooltip("旋回抵抗")]
        private float _turnDrag = 0.5f;

        [SerializeField, Tooltip("移動抵抗")]
        private float _moveDrag = 0.5f;
    }
}
