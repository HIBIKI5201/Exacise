using System;
using System.Collections.Generic;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace AbeShootingGame
{
    [Serializable]
    public class BulletPool
    {
        public BulletPool(BulletManager prefab)
        {
            _bulletPrefab = prefab;

            _pool = new(
                createFunc: () => Object.Instantiate(_bulletPrefab),
                actionOnDestroy: bullet => Object.Destroy(bullet)
                );
        }

        public IReadOnlyList<BulletManager> ActiveBullets => _activeBullets;

        public BulletManager Get()
        {
            BulletManager bullet = _pool.Get();
            bullet.gameObject.SetActive(true);
            _activeBullets.Add(bullet);
            bullet.OnHitted += Release;
            return bullet;
        }

        public void Release(BulletManager bullet)
        {
            if (_activeBullets.Remove(bullet))
            {
                _pool.Release(bullet);
                bullet.gameObject.SetActive(false);
            }
        }

        private readonly BulletManager _bulletPrefab;
        private readonly ObjectPool<BulletManager> _pool;
        private readonly List<BulletManager> _activeBullets = new();
    }
}
