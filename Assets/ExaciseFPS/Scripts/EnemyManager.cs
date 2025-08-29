using UnityEngine;

namespace ExaciseFPS.SourceCode
{
    /// <summary>
    ///   敵のマネージャー
    /// </summary>
    public class EnemyManager : MonoBehaviour, IHitable
    {
        [SerializeField]
        private float _maxHealth = 100f;

        [SerializeField]
        private ParticleSystem _hitParticle;

        private float _currentHealth;
        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void Hit(float damage)
        {
            _currentHealth -= damage;
            _hitParticle?.Play();

            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
