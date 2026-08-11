using UnityEngine;

namespace NavyGame.Runtime
{
    [CreateAssetMenu(fileName = nameof(TurretStatus), menuName = "NavyGame/" + nameof(TurretStatus))]
    public class TurretStatus : ScriptableObject
    {
        public float FireRate => _fireRate;
        public float Range => _range;
        public float MaxSpreadAngle => _maxSpreadAngle;

        [SerializeField, Tooltip("発射間隔 (秒)")]
        private float _fireRate = 1f;
        [SerializeField, Tooltip("射程距離 (メートル)")]
        private float _range = 100f;
        [SerializeField, Tooltip("弾丸の最大拡散角度 (度)")]
        private float _maxSpreadAngle = 5f;
    }
}
