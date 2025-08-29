using System;
using UnityEngine;

namespace GridDungeon.Scripts
{
    /// <summary>
    /// キャラクターをグリッドベースで移動させるためのクラス。
    /// IGridObjectインターフェースを実装し、グリッド上のオブジェクトとして振る舞います。
    /// </summary>
    public class CharacterMover : MonoBehaviour, IGridObject
    {
        /// <summary>
        /// キャラクターの位置が変更されたときに発行されるイベント。
        /// 引数1: 旧座標, 引数2: 新座標
        /// </summary>
        public event Action<Vector2Int, Vector2Int> OnPositionChanged;

        /// <summary>
        /// グリッド上の現在の座標。
        /// </summary>
        public Vector2Int Position => new(_currentPosOnGrid.x, _currentPosOnGrid.z);

        [SerializeField, Tooltip("ワールド設定")]
        private WorldConfig _worldConfig;

        private Vector3Int _currentPosOnGrid;
        private GridManager _gridManager;

        private void Awake()
        {
            Debug.Assert(_worldConfig != null, "WorldConfigがアタッチされていません。");
        }

        private void Start()
        {
            if (_worldConfig == null) return;

            // GridManagerをシーンから検索して取得
            _gridManager = FindAnyObjectByType<GridManager>();
            if (_gridManager == null)
            {
                Debug.LogError("GridManagerがシーン内に見つかりません。", this);
                return;
            }

            // 開始時にキャラクターの位置を最も近いグリッドにスナップさせる
            SnapToGrid();

            // GridManagerに自身を登録する
            _gridManager.Register(this, destroyCancellationToken);
        }

        /// <summary>
        /// 指定された方向にキャラクターを移動させます。
        /// </summary>
        /// <param name="direction">移動する方向</param>
        public void MoveTo(Vector2Int direction)
        {
            if (_worldConfig == null || _gridManager == null) return;

            // 新しい目標座標を計算
            Vector2Int newPos = new Vector2Int(_currentPosOnGrid.x + direction.x, _currentPosOnGrid.z + direction.y);

            // GridManagerで移動可能か検証
            if (_gridManager.IsValidCell(newPos))
            {
                // グリッド上の座標を更新
                UpdateGridPosition(newPos);
                // ワールド座標を更新
                UpdateWorldPosition();
            }
        }

        /// <summary>
        /// 現在のワールド座標をグリッドにスナップさせます。
        /// </summary>
        private void SnapToGrid()
        {
            // ワールド座標をグリッドスケールで割り、グリッド座標に変換
            Vector3 gridPos = transform.position / _worldConfig.GridScale;
            Vector3Int snappedGridPos = Vector3Int.FloorToInt(gridPos);

            // グリッド上の座標を更新
            UpdateGridPosition(new Vector2Int(snappedGridPos.x, snappedGridPos.z));
            // ワールド座標を更新
            UpdateWorldPosition();
        }

        /// <summary>
        /// グリッド上の座標を更新し、OnPositionChangedイベントを発行します。
        /// </summary>
        /// <param name="newPos">新しいグリッド座標</param>
        private void UpdateGridPosition(Vector2Int newPos)
        {
            var oldPos = new Vector2Int(_currentPosOnGrid.x, _currentPosOnGrid.z);
            if (oldPos == newPos) return;

            OnPositionChanged?.Invoke(oldPos, newPos);
            _currentPosOnGrid = new Vector3Int(newPos.x, 0, newPos.y);
        }

        /// <summary>
        /// グリッド座標に基づいてワールド座標を更新します。
        /// </summary>
        private void UpdateWorldPosition()
        {
            // グリッド座標にオフセットを加え、スケールを掛けてワールド座標を算出
            Vector3 pos = (Vector3)_currentPosOnGrid + new Vector3(_worldConfig.GridOffset.x, 0, _worldConfig.GridOffset.y);
            pos *= _worldConfig.GridScale;
            transform.position = pos;
        }
    }
}
