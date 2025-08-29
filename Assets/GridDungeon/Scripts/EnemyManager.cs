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

        public void MoveEnemies()
        {
            foreach (var enemy in _enemies)
            {
                Vector2Int pos = AIUtility.GetNextPosition(
                    _player.Position,
                    enemy.Position,
                    _gridManager);

                enemy.MoveTo(pos);
            }
        }
    }
}
