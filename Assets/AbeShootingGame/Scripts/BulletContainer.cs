using System.Collections.Generic;
using UnityEngine;

namespace AbeShootingGame
{
    public class BulletContainer : MonoBehaviour
    {
        private void Awake()
        {
            _bulletPool = new(_bulletPrefab);
        }

        public void Fire(Vector3 position)
        {
            BulletManager bullet = _bulletPool.Get();
            bullet.Fire(Vector3.right);
            bullet.transform.position = position;
        }

        [SerializeField]
        private BulletManager _bulletPrefab;

        private BulletPool _bulletPool;

        public bool IsHitBullet(Vector3 position, float radius, out BulletManager hitBullet)
        {

            foreach (BulletManager b in _bulletPool.ActiveBullets)
            {
                float sumRadius = b.CollisionRadius + radius;
                if (Vector3.Distance(position, b.transform.position) < sumRadius)
                {
                    hitBullet = b;
                    return true;
                }
            }

            hitBullet = null;
            return false;
        }

    }
}
