using System;
using System.Threading;
using UnityEngine;

namespace GridDungeon.Scripts
{
    public class CharacterBattler : MonoBehaviour
    {
        public event Action<int> OnHpChanged;
        public event Action<CharacterBattler> OnDied;

        public void TakeDamage(int damage)
        {
            _currentHp -= damage;
            if (_currentHp <= 0)
            {
                _currentHp = 0;
                Die();
            }

            HitEffect();

            OnHpChanged?.Invoke(_currentHp);
        }

        public void Die()
        {
            OnDied?.Invoke(this);
        }

        public void Attack(CharacterBattler target)
        {
            target.TakeDamage(_attackPower);
        }

        [SerializeField]
        private int _maxHp = 100;
        [SerializeField]
        private int _attackPower = 10;

        [Space]
        [SerializeField]
        private float _hitEffectDuration = 0.1f;
        [SerializeField]
        private Color _hitEffectColor = Color.red;

        private int _currentHp;

        private CancellationTokenSource _hitCts = new();
        private Renderer _renderer;

        private void Awake()
        {
            _currentHp = _maxHp;
            _renderer = GetComponentInChildren<Renderer>();
        }

        private async void HitEffect()
        {
            if (_renderer == null) return;
            
            _hitCts.Cancel();
            _hitCts = new CancellationTokenSource();
            
            Material material = _renderer.material;
            Color originalColor = material.color;
            material.color = _hitEffectColor;

            try
            {
                // 指定された時間だけ待機
                await Awaitable.WaitForSecondsAsync(_hitEffectDuration, _hitCts.Token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            finally
            {
                material.color = originalColor;
            }

        }
    }
}
