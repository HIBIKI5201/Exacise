using UnityEngine;

namespace NavyGame.Runtime
{
    public class BulletMover : MonoBehaviour
    {
        public void Init()
        {
            Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        [SerializeField]
        private float _speed = 100f;
        [SerializeField]
        private float _lifeTime = 2f;
    }
}
