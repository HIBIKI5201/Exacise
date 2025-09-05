using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GridDungeon.Scripts
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private CharacterData _player;
        [SerializeField]
        private GridManager _gridManager;
        [SerializeField]
        private List<CharacterData> _enemies;

        private void Awake()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Battler.OnDied += HandleEnemyDied;
            }
        }

        public async Task MoveEnemies()
        {
            Task<(CharacterData, List<Vector2Int>)>[] pathTasks =
                new Task<(CharacterData, List<Vector2Int>)>[_enemies.Count];

            for (int i = 0; i < pathTasks.Length; i++)
            {
                int index = i; // キャプチャ用のローカル変数

                pathTasks[index] = Task.Run(
                    async () =>
                {
                    await Awaitable.BackgroundThreadAsync();

                    CharacterMover enemy = _enemies[index].Mover;

                    Vector2Int playerPos = _gridManager.ApplyOffset(_player.Mover.Position);
                    Vector2Int enemyPos = _gridManager.ApplyOffset(enemy.Position);

                    List<Vector2Int> path = AIUtility.GetPath(
                        _gridManager.IsUsedGrid,
                        enemyPos, playerPos);

                    return (_enemies[index], path);
                });
            }

            await Task.WhenAll(pathTasks);

            Task[] moveTasks = new Task[pathTasks.Length];
            for (int i = 0; i < pathTasks.Length; i++)
            {
                var task = pathTasks[i];
                (CharacterData enemy, List<Vector2Int> path) = task.Result;

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
                    moveTasks[i] = enemy.Mover.MoveTo(dir);

                    continue;
                }

                enemy.Battler.Attack(_player.Battler);

                moveTasks[i] = Task.CompletedTask;
            }

            await Task.WhenAll(moveTasks); // すべての敵の移動が完了するまで待機
        }

        public CharacterBattler GetEnemyAtPosition(Vector2Int position)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.Mover.Position == position)
                {
                    return enemy.Battler;
                }
            }

            return null;
        }

        private void OnValidate()
        {
            _player.Initialize();
            foreach (var enemy in _enemies)
            {
                enemy.Initialize();
            }
        }

        private void HandleEnemyDied(CharacterBattler battler)
        {
            CharacterData data = _enemies.Find(e => e.Battler == battler);
            _enemies.Remove(data);

            Destroy(battler.gameObject);
        }

        [Serializable]
        private class CharacterData
        {
            public CharacterMover Mover => _mover;
            public CharacterBattler Battler => _battler;

            public void Initialize()
            {
                if (_root == null) return;

                _mover = _root.GetComponent<CharacterMover>();
                _battler = _root.GetComponent<CharacterBattler>();
            }

            [SerializeField]
            private GameObject _root;

            private CharacterMover _mover;
            private CharacterBattler _battler;
        }
    }
}
