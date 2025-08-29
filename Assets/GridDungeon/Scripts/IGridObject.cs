using System;
using UnityEngine;

namespace GridDungeon.Scripts
{
    /// <summary>
    /// グリッド上に配置可能なオブジェクトを表すインターフェース。
    /// </summary>
    public interface IGridObject
    {
        /// <summary>
        /// オブジェクトのグリッド座標が変更されたときに発行されるイベント。
        /// Action<旧座標, 新座標>
        /// </summary>
        event Action<Vector2Int, Vector2Int> OnPositionChanged;

        /// <summary>
        /// オブジェクトの現在のグリッド座標。
        /// </summary>
        Vector2Int Position { get; }
    }
}