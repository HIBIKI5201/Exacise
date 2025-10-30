using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Scripts
{
    public class CharacterAnimator : MonoBehaviour
    {
        public async Task FadeIn(float duration = 0.5f, CancellationToken token = default)
        {
            // 経過時間に基づいてアルファ値を計算。
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float alpha = Mathf.Clamp01(elapsed / duration);

                ChangeAllColorAlpha(alpha);
                elapsed += Time.deltaTime;

                await Awaitable.NextFrameAsync(token);
            }

            // 最後にアルファ値を1に設定。
            ChangeAllColorAlpha(1f);
        }

        public async Task FadeOut(float duration = 0.5f, CancellationToken token = default)
        {
            // 経過時間に基づいてアルファ値を計算。
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float alpha = 1f - Mathf.Clamp01(elapsed / duration);
                ChangeAllColorAlpha(alpha);
                elapsed += Time.deltaTime;
                await Awaitable.NextFrameAsync(token);
            }
            // 最後にアルファ値を0に設定。
            ChangeAllColorAlpha(0f);
        }

        private Animator _animator;
        private SpriteRenderer[] _renderers;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        private void ChangeAllColorAlpha(float a)
        {
            foreach (var renderer in _renderers)
            {
                Color color = renderer.color;
                color.a = a;
                renderer.color = color;
            }
        }
    }
}