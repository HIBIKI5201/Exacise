using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridDungeon.Scripts
{
    /// <summary>
    ///     AIの動作に関するユーティリティクラス。
    /// </summary>
    public static class AIUtility
    {
        /// <summary>
        ///    A*アルゴリズムを使用して、2Dグリッド上での最短経路を見つけます。
        /// </summary>
        /// <param name="grid">通行可能なマスを true、壁や障害物を false で表す2D配列</param>
        /// <param name="start">開始座標</param>
        /// <param name="goal">ゴール座標</param>
        /// <returns>経路の座標リスト。経路が見つからなければ null</returns>
        public static List<Vector2Int> GetPath(bool[,] grid, Vector2Int start, Vector2Int goal)
        {
            // ゴールがグリッドの範囲外であれば、経路なしと判断
            if (!IsInside(grid, goal))
            {
                return null;
            }

            // MinHeapでオープンリストを管理。f値最小のノードをO(log n)で取得
            MinHeap openList = new();

            // クローズドリストは探索済み座標を保持
            HashSet<Vector2Int> closedList = new();

            // 全ノードを座標をキーに辞書で管理
            Dictionary<Vector2Int, Node> allNodes = new();

            // 開始ノードを初期化
            Node startNode = new Node(start, 0, Manhattan(start, goal), Node.NoParent);
            openList.Insert(startNode);
            allNodes.Add(start, startNode);

            // 4方向の移動ベクトル
            ReadOnlySpan<Vector2Int> directions = stackalloc Vector2Int[]
            {
                Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
            };

            // 未探索のノードがなくなるまでループ
            while (openList.Count > 0)
            {
                Node current = openList.PopMin(); // f値最小ノードを取得
                closedList.Add(current.Pos);

                // ゴールに到達した場合は経路を復元して返す
                if (current.Pos == goal)
                {
                    return ReconstructPath(allNodes, current);
                }

                // 4方向の隣接マスをチェック
                foreach (var dir in directions)
                {
                    Vector2Int neighborPos = current.Pos + dir;

                    if (!IsPassable(grid, neighborPos, goal)) continue; // 通行不可マスはスキップ
                    if (closedList.Contains(neighborPos)) continue;      // 探索済みはスキップ

                    // 移動コストを計算
                    int newG = current.CostFromStart + 1;

                    // より良い経路が見つかっていないかチェック
                    if (allNodes.TryGetValue(neighborPos, out Node existingNode) && existingNode.CostFromStart <= newG)
                    {
                        continue;
                    }

                    // 新規ノード、またはより良い経路としてオープンリストに追加
                    Node neighborNode = new Node(
                        neighborPos,
                        newG,
                        Manhattan(neighborPos, goal),
                        current.Pos
                    );

                    openList.Insert(neighborNode);
                    allNodes[neighborPos] = neighborNode;
                }
            }

            // 経路が見つからなければnullを返す
            return null;
        }

        /// <summary>
        ///     グリッド内に収まっているか確認
        /// </summary>
        private static bool IsInside(bool[,] grid, Vector2Int pos)
            => pos.x >= 0 && pos.y >= 0 && pos.x < grid.GetLength(0) && pos.y < grid.GetLength(1);

        /// <summary>
        ///     通行可能か確認
        /// </summary>
        private static bool IsPassable(bool[,] grid, Vector2Int pos, Vector2Int goal)
        {
            // 目的地は常に通行可能とみなす
            if (pos == goal) return true;

            // 境界内で、かつ使用中でない（壁でない）ことを確認
            return IsInside(grid, pos) && !grid[pos.x, pos.y];
        }

        /// <summary>
        ///     2点間のマンハッタン距離を計算
        /// </summary>
        private static int Manhattan(Vector2Int a, Vector2Int b)
            => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

        /// <summary>
        ///     経路を親ノード情報から復元
        /// </summary>
        private static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Node> nodes, Node lastNode)
        {
            var path = new List<Vector2Int>();
            Node currentNode = lastNode;

            // 親ノードをたどりながら経路を追加
            while (currentNode.ParentPos != Node.NoParent)
            {
                path.Add(currentNode.Pos);
                if (nodes.TryGetValue(currentNode.ParentPos, out Node parentNode))
                {
                    currentNode = parentNode;
                }
                else
                {
                    // 親が見つからなければ終了（始点に到達）
                    break;
                }
            }

            path.Add(currentNode.Pos); // 始点ノードを追加

            // 開始→ゴール順に反転
            path.Reverse();
            return path;
        }

        /// <summary>
        ///     経路探索用のノード構造体。
        /// </summary>
        private readonly struct Node
        {
            public static readonly Vector2Int NoParent = new Vector2Int(-1, -1);

            public Node(Vector2Int pos, int g, int h, Vector2Int parentPos)
            {
                _pos = pos;
                _costFromStart = g;
                _heuristicCost = h;
                _parentPos = parentPos;
            }

            // ノード座標
            public Vector2Int Pos => _pos;

            // f値 = g + h
            public int TotalEstimatedCost => _costFromStart + _heuristicCost;

            // スタートからの距離
            public int CostFromStart => _costFromStart;

            // ゴールまでの推定距離
            public int HeuristicCost => _heuristicCost;

            // 親ノードの座標 (NoParentならなし)
            public Vector2Int ParentPos => _parentPos;

            private readonly Vector2Int _pos;
            private readonly int _costFromStart;
            private readonly int _heuristicCost;
            private readonly Vector2Int _parentPos;
        }


        /// <summary>
        ///     最小ヒープによる優先度付きキュー実装。
        ///     f値最小のノードを高速に取得可能。
        /// </summary>
        private class MinHeap
        {
            private readonly List<Node> _heap = new();

            public int Count => _heap.Count;

            /// <summary>
            ///     ノードをヒープに追加
            /// </summary>
            public void Insert(Node node)
            {
                _heap.Add(node);
                int i = _heap.Count - 1;
                while (i > 0)
                {
                    int parent = (i - 1) / 2;
                    if (_heap[i].TotalEstimatedCost >= _heap[parent].TotalEstimatedCost) break;

                    Swap(i, parent);
                    i = parent;
                }
            }

            /// <summary>
            ///     最小f値ノードを取り出す
            /// </summary>
            public Node PopMin()
            {
                Node min = _heap[0];
                _heap[0] = _heap[_heap.Count - 1];
                _heap.RemoveAt(_heap.Count - 1);
                Heapify(0);
                return min;
            }

            /// <summary>
            ///     ヒープ条件を維持
            /// </summary>
            private void Heapify(int i)
            {
                int left = 2 * i + 1;
                int right = 2 * i + 2;
                int smallest = i;

                // 左右の子ノードと比較して最小値を見つける
                if (left < _heap.Count && _heap[left].TotalEstimatedCost < _heap[smallest].TotalEstimatedCost) smallest = left;
                if (right < _heap.Count && _heap[right].TotalEstimatedCost < _heap[smallest].TotalEstimatedCost) smallest = right;

                if (smallest != i)
                {
                    Swap(i, smallest);
                    Heapify(smallest);
                }
            }

            /// <summary>
            ///     ノードの入れ替え
            /// </summary>
            private void Swap(int i, int j) => (_heap[i], _heap[j]) = (_heap[j], _heap[i]);
        }
    }
}
