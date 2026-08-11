using UnityEngine;

namespace Beyblade
{
    [RequireComponent(typeof(Rigidbody))]
    public class Blade : MonoBehaviour
    {
        private const float GRAVITY = -9.81f;

        [SerializeField] float _reflectForce = 5f;
        [SerializeField] float _gravityScale = 1f;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            Physics.gravity = GRAVITY * _gravityScale * Vector3.up;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_rigidbody == null) { return; }
            if (!collision.gameObject.TryGetComponent<Blade>(out _)) { return; }

            Vector3 diff = (collision.transform.position - transform.position);
            diff.y = 0;
            Vector3 dir = diff.normalized;
            dir *= -1f;

            _rigidbody.AddForce(dir * _reflectForce, ForceMode.Impulse);
        }
    }
}
