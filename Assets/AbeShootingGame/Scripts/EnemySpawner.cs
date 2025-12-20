using UnityEngine;

namespace AbeShootingGame
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private EnemyManager _enemyPrefab;
        [SerializeField]
        private BulletContainer _bulletContainer;
        [SerializeField]
        private Transform[] _spawnPoints;

        private void Start()
        {
            foreach (var spawnPoint in _spawnPoints)
            {
                SpawnEnemy(spawnPoint.position);
            }
        }

        private void SpawnEnemy(Vector3 position)
        {
            EnemyManager enemyManager = Instantiate(_enemyPrefab);
            enemyManager.transform.position = position;
            enemyManager.Init(_bulletContainer);
        }
    }
}
