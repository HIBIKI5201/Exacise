using UnityEngine;

namespace AbeShootingGame
{
    public class EnemyManager : MonoBehaviour
    {
        public void Init(BulletContainer bulletContainer)
        {
            _bulletContainer = bulletContainer;
        }

        [SerializeField]
        private float _collisionRaidus;

        private BulletContainer _bulletContainer;

        private void Update()
        {
            if (_bulletContainer.IsHitBullet(transform.position, _collisionRaidus, out BulletManager bullet))
            {
                bullet.Hit();
                Destroy(gameObject);
            }
        }
    }
}
