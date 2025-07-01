using UnityEngine;

namespace ExaciseFPS.SourceCode
{
    /// <summary>
    ///   �G�̃}�l�[�W���[
    /// </summary>
    public class EnemyManager : MonoBehaviour, IHitable
    {
        [SerializeField]
        private float _maxHealth = 100f;

        private float _currentHealth;
        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void Hit(float damage)
        {
            _currentHealth -= damage;
            
            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
