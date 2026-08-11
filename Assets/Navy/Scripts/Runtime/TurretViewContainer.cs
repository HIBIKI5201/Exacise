using System;
using UnityEngine;
using UnityEngine.Events;

namespace NavyGame.Runtime
{
    /// <summary>
    ///     タレットのオブジェクトデータを保持するコンテナ。
    /// </summary>
    public class TurretViewContainer : MonoBehaviour
    {
        public BulletMover Bullet => _bullet;
        public TurretStatus Status => _status;

        public Transform H => _h;
        public Transform V => _v;
        public Transform Pivot => _pivot;

        public Transform Muzzle => _muzzle;

        public void InvokeOnShot() => _onShot?.Invoke();

        [SerializeField]
        private UnityEvent _onShot;

        [Header("Status")]
        [SerializeField, Tooltip("弾丸のオブジェクト")]
        private BulletMover _bullet;
        [SerializeField, Tooltip("ステータス")]
        private TurretStatus _status;

        [Header("References")]
        [SerializeField, Tooltip("水平回転用のTransform")]
        private Transform _h;
        [SerializeField, Tooltip("垂直回転用のTransform")]
        private Transform _v;

        [SerializeField, Tooltip("ピボットポイントのTransform")]
        private Transform _pivot;

        [SerializeField, Tooltip("銃口のTransform")]
        private Transform _muzzle;
    }
}
