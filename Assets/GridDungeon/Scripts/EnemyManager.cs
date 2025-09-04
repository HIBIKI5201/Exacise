using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GridDungeon.Scripts
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private CharacterMover _player;
        [SerializeField]
        private GridManager _gridManager;
        [SerializeField]
        private CharacterMover[] _enemies;

        public async Task MoveEnemies()
        {
            Task<(CharacterMover, List<Vector2Int>)>[] pathTasks = 
                new Task<(CharacterMover, List<Vector2Int>)>[_enemies.Length];

            for (int i = 0; i < pathTasks.Length; i++)
            {
                int index = i; // キャプチャ用のローカル変数

                pathTasks[index] = Task<(CharacterMover, List<Vector2Int>)>.Run(
                    async () =>
                {
                    await Awaitable.BackgroundThreadAsync();

                    CharacterMover enemy = _enemies[index];

                    Vector2Int playerPos = _gridManager.ApplyOffset(_player.Position);
                    Vector2Int enemyPos = _gridManager.ApplyOffset(enemy.Position);

                    List<Vector2Int> path = AIUtility.GetPath(
                        _gridManager.IsUsedGrid,
                        enemyPos, playerPos);

                    return (enemy, path);
                });
            }

            await Task.WhenAll(pathTasks);

            Task[] moveTasks = new Task[pathTasks.Length];
            for (int i = 0; i < pathTasks.Length; i++)
            {
                var task = pathTasks[i];
                (CharacterMover enemy, List<Vector2Int> path)= task.Result;

                // 経路が見つからない、または移動不要な場合はスキップ
                if (path == null)
                {
                    Debug.Log("No path found or already at the target.");
                    moveTasks[i] = Task.CompletedTask;
                    continue;
                }

                // 経路の最初のステップに向かって移動
                if (2 < path.Count)
                {
                    Vector2Int dir = path[1] - path[0];
                    moveTasks[i] = enemy.MoveTo(dir);

                    continue;
                }

                Debug.Log("Path too short to move.");
                moveTasks[i] = Task.CompletedTask;
            }

            await Task.WhenAll(moveTasks); // すべての敵の移動が完了するまで待機
        }
    }
}
