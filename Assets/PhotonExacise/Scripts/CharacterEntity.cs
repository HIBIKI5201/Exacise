using System;

namespace PhotonExacise.Scripts
{
    public class CharacterEntity
    {
        public CharacterEntity(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
        }

        /// <summary> 第一は現在値、第二は最大値 </summary>
        public event Action<float, float> OnHealthChanged;

        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        private readonly float _maxHealth;
        private float _currentHealth;
    }
}
