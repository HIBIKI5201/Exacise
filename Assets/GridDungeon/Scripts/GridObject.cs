using System;
using UnityEngine;

namespace GridDungeon.Scripts
{
    public class GridObject : MonoBehaviour, IGridObject
    {
        public Vector2Int Position => _position;

        public event Action<Vector2Int, Vector2Int> OnPositionChanged;

        [SerializeField]
        private WorldConfig _worldConfig;

        private Vector2Int _position;

        private void Awake()
        {
            if (_worldConfig == null)
            {
                Debug.LogError("WorldConfigがアタッチされていません。", this);
                return;
            }

            Vector3 gridPos = transform.position / _worldConfig.GridScale;
            Vector3Int snappedGridPos = Vector3Int.FloorToInt(gridPos);
            _position = new Vector2Int(snappedGridPos.x, snappedGridPos.z);
        }

        private void Start()
        {
            GridManager gridManager = FindAnyObjectByType<GridManager>();
            gridManager.Register(this, destroyCancellationToken);

            Vector3 pos = new Vector3(_position.x, transform.position.y, _position.y)
                + new Vector3(_worldConfig.GridOffset.x, 0, _worldConfig.GridOffset.y);
            pos *= _worldConfig.GridScale;

            transform.position = pos;
        }
    }
}
