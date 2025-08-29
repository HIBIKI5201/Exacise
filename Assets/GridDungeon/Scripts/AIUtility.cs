using System.Collections.Generic;
using UnityEngine;

namespace GridDungeon.Scripts
{
    /// <summary>
    /// AIの動作に関するユーティリティクラス。
    /// </summary>
    public static class AIUtility
    {
        /// <summary>
        /// A*アルゴリズムで使用するノードを表す内部クラス。
        /// </summary>
        private class Node
        {
            public Vector2Int Position { get; }
            public int GCost { get; set; } // スタートノードからのコスト
            public int HCost { get; set; } // ターゲットノードまでの推定コスト
            public int FCost => GCost + HCost; // GCostとHCostの合計
            public Node Parent { get; set; }

            public Node(Vector2Int position)
            {
                Position = position;
            }
        }

        /// <summary>
        /// A*アルゴリズムを使用して、ターゲットへの次の移動位置を計算します。
        /// </summary>
        /// <param name="target">ターゲットの座標</param>
        /// <param name="self">自身の座標</param>
        /// <param name="gridManager">グリッドマネージャー</param>
        /// <returns>次に移動すべき座標。パスが見つからない場合は自身の座標を返します。</returns>
        public static Vector2Int GetNextPosition(Vector2Int target, Vector2Int self, GridManager gridManager)
        {
            if (gridManager == null)
            {
                return self; // GridManagerがなければ移動しない
            }

            var startNode = new Node(self);
            var targetNode = new Node(target);

            var openList = new List<Node> { startNode };
            var closedSet = new HashSet<Vector2Int>();

            startNode.GCost = 0;
            startNode.HCost = CalculateHeuristic(self, target);

            while (openList.Count > 0)
            {
                // openListからFコストが最も低いノードを取得
                Node currentNode = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].FCost < currentNode.FCost || (openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost))
                    {
                        currentNode = openList[i];
                    }
                }

                openList.Remove(currentNode);
                closedSet.Add(currentNode.Position);

                // ターゲットに到達した場合
                if (currentNode.Position == target)
                {
                    return RetracePath(startNode, currentNode);
                }

                // 隣接ノードを評価
                foreach (var neighbourOffset in GetNeighbours())
                {
                    Vector2Int neighbourPos = currentNode.Position + neighbourOffset;

                    // 既に評価済みか、移動不可能なセルかを確認
                    // 注意：ターゲット自身は移動可能とみなす
                    if (closedSet.Contains(neighbourPos) || (neighbourPos != target && !gridManager.IsValidCell(neighbourPos)))
                    {
                        continue;
                    }
                    
                    var openNode = openList.Find(n => n.Position == neighbourPos);
                    int newMovementCostToNeighbour = currentNode.GCost + CalculateHeuristic(currentNode.Position, neighbourPos);

                    // openListにないか、より良い経路の場合
                    if (openNode == null || newMovementCostToNeighbour < openNode.GCost)
                    {
                        var neighbourNode = openNode ?? new Node(neighbourPos);
                        neighbourNode.Parent = currentNode;
                        neighbourNode.GCost = newMovementCostToNeighbour;
                        neighbourNode.HCost = CalculateHeuristic(neighbourPos, target);
                        
                        if (openNode == null)
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }

            return self; // パスが見つからなかった場合
        }

        /// <summary>
        /// ゴールノードから親をたどってパスを再構築し、最初のステップを返します。
        /// </summary>
        private static Vector2Int RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();

            // 次の1歩を返す。パスがなければ現在地を返す。
            return path.Count > 0 ? path[0].Position : startNode.Position;
        }

        /// <summary>
        /// 2点間のヒューリスティックコスト（マンハッタン距離）を計算します。
        /// </summary>
        private static int CalculateHeuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        /// <summary>
        /// 隣接する4方向のオフセットを取得します。
        /// </summary>
        private static IEnumerable<Vector2Int> GetNeighbours()
        {
            yield return new Vector2Int(0, 1);  // 上
            yield return new Vector2Int(0, -1); // 下
            yield return new Vector2Int(1, 0);  // 右
            yield return new Vector2Int(-1, 0); // 左
        }
    }
}