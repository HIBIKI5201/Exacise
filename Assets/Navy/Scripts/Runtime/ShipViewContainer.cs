using UnityEngine;

namespace NavyGame.Runtime
{
    /// <summary>
    ///     船のビューコンテナ。船に搭載されているタレットのビューコンテナを保持する。
    /// </summary>
    public class ShipViewContainer : MonoBehaviour
    {
        public ShipStatus Status => _status;
        public TurretViewContainer[] Turrets => _turrets;

        [SerializeField]
        private ShipStatus _status;
        [SerializeField]
        private TurretViewContainer[] _turrets;

        private void OnValidate()
        {
            _turrets = GetComponentsInChildren<TurretViewContainer>();
        }
    }
}
