using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace GridDungeon.Scripts
{
    /// <summary>
    /// グリッドシステムを管理します。
    /// グリッドのサイズ、使用状況、オブジェクトの登録などを扱います。
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        /// <summary>
        /// グリッドのサイズ。
        /// </summary>
        public Vector2Int Size => _size;

        /// <summary>
        /// グリッドにオブジェクトを登録します。
        /// </summary>
        /// <param name="obj">登録するIGridObject</param>
        /// <param name="token">キャンセルトークン</param>
        public void Register(IGridObject obj, CancellationToken token = default)
        {
            // オブジェクトの位置変更イベントにコールバックを登録
            Action<Vector2Int, Vector2Int> action = (oldPos, newPos) =>
            {
                if (token.IsCancellationRequested) return;

                // グリッドマネージャの位置を考慮したオフセットを適用
                oldPos = ApplyOffset(oldPos);
                newPos = ApplyOffset(newPos);

                // 古い位置を「未使用」に、新しい位置を「使用中」に更新
                if (IsWithinBounds(oldPos))
                    _isUsedGrid[oldPos.x, oldPos.y] = false;
                if (IsWithinBounds(newPos))
                    _isUsedGrid[newPos.x, newPos.y] = true;
            };

            var initialPos = ApplyOffset(obj.Position);
            if (IsWithinBounds(initialPos))
                _isUsedGrid[initialPos.x, initialPos.y] = true;

            obj.OnPositionChanged += action;
            // オブジェクトが破棄される際にイベントの登録を解除
            token.Register(() =>
                {
                    obj.OnPositionChanged -= action;

                    var pos = ApplyOffset(obj.Position);
                    if (IsWithinBounds(pos))
                        _isUsedGrid[pos.x, pos.y] = false;
                });
        }

        /// <summary>
        /// 指定された座標が移動可能かつ有効なセルであるかを確認します。
        /// </summary>
        /// <param name="pos">確認する座標</param>
        /// <returns>有効な場合はtrue</returns>
        public bool IsValidCell(Vector2Int pos)
        {
            var offsetPos = ApplyOffset(pos);

            // 境界内であり、かつ使用中でないかを確認
            return IsWithinBounds(offsetPos) && !_isUsedGrid[offsetPos.x, offsetPos.y];
        }


        /// <summary>
        /// ワールド座標をグリッドのローカル座標に変換します。
        /// </summary>
        public Vector2Int ApplyOffset(Vector2Int pos)
        {
            // GridManager自身のワールド座標を引くことで、ローカルなグリッド座標に変換
            pos -= _positionOnGrid;
            return pos;
        }

        public bool[,] IsUsedGrid => _isUsedGrid;

        [SerializeField, Tooltip("グリッドのサイズ")]
        private Vector2Int _size = new(10, 10);

        private Vector2Int _positionOnGrid;

        // グリッドの各セルが使用されているかを管理する2次元配列
        private bool[,] _isUsedGrid;

        private void Awake()
        {
            // 実行時にグリッド配列を初期化
            InitializeGrid();

            _positionOnGrid = new Vector2Int(
                Mathf.FloorToInt(transform.position.x),
                Mathf.FloorToInt(transform.position.z));
        }

        private void OnValidate()
        {
            // Inspectorでの値変更時にグリッド配列を再生成
            // これにより、エディタでのサイズ変更が即座に反映されます。
            InitializeGrid();
        }

        /// <summary>
        /// グリッド配列を初期化または再生成します。
        /// </summary>
        private void InitializeGrid()
        {
            // 既に適切なサイズの配列が存在する場合は何もしない
            if (_isUsedGrid != null && _isUsedGrid.GetLength(0) == _size.x && _isUsedGrid.GetLength(1) == _size.y)
            {
                return;
            }
            _isUsedGrid = new bool[_size.x, _size.y];
        }

        /// <summary>
        /// 指定された座標がグリッドの境界内にあるかを確認します。
        /// </summary>
        /// <param name="pos">確認する座標</param>
        /// <returns>境界内の場合はtrue</returns>
        private bool IsWithinBounds(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < _size.x && pos.y >= 0 && pos.y < _size.y;
        }

        private void OnDrawGizmos()
        {
            // エディタ実行中でなければグリッドを初期化
            if (!Application.isPlaying)
            {
                InitializeGrid();
            }
            if (_isUsedGrid == null) return;

            Vector3 offset = transform.position;
            for (int x = 0; x < _size.x; x++)
            {
                for (int z = 0; z < _size.y; z++)
                {
                    // 各セルの状態に応じてギズモの色を設定
                    Gizmos.color = _isUsedGrid[x, z] ? Color.red : Color.green;
                    Vector3 cellCenter = new Vector3(x + 0.5f, 0, z + 0.5f);
                    Gizmos.DrawWireCube(cellCenter + offset, new Vector3(1, 0.1f, 1) * 0.9f);
                }
            }
        }
    }
}