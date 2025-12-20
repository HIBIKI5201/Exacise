using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Scripts
{
    public class CharacterAnimator : MonoBehaviour
    {
        public string Name => name;
        
        public async Task FadeIn(float duration = 0.5f, IPauseHandler ph = null, CancellationToken token = default)
        {
            try
            {
                // 経過時間に基づいてアルファ値を計算。
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    float alpha = Mathf.Clamp01(elapsed / duration);

                    Debug.Log($"FadeIn Alpha: {alpha}\n{elapsed}/{duration}");
                    ChangeAllColorAlpha(alpha);
                    elapsed += Time.deltaTime;

                    await Awaitable.NextFrameAsync(token);
                    if (ph != null) { await ph.WaitResumeAsync(token); }
                }
            }
            finally
            {
                // 最後にアルファ値を1に設定。
                ChangeAllColorAlpha(1f);
            }
        }

        public async Task FadeOut(float duration = 0.5f, IPauseHandler ph = null, CancellationToken token = default)
        {
            try
            {
                // 経過時間に基づいてアルファ値を計算。
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    float alpha = 1f - Mathf.Clamp01(elapsed / duration);
                    ChangeAllColorAlpha(alpha);
                    elapsed += Time.deltaTime;

                    await Awaitable.NextFrameAsync(token);
                    if (ph != null) { await ph.WaitResumeAsync(token); }
                }
            }
            finally
            {
                // 最後にアルファ値を0に設定。
                ChangeAllColorAlpha(0f);
            }
        }

        public async Task PlayAnimationAsync(string animation, IPauseHandler ph, CancellationToken token = default)
        {
            _animator.Play(animation);

            // アニメーションの長さを取得。
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float animationLength = stateInfo.length;

            try
            {
                await Awaitable.WaitForSecondsAsync(animationLength, token);
                if (ph != null) { await ph.WaitResumeAsync(token); }
            }
            catch (OperationCanceledException)
            {
                _animator.SetTrigger("Reset");
            }
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
            ExcuteForAllRenderer(renderer =>
            {
                Color color = renderer.color;
                color.a = a;
                renderer.color = color;
            });
        }

        private void ExcuteForAllRenderer(Action<SpriteRenderer> action)
        {
            foreach(SpriteRenderer renderer in _renderers)
            {
                action(renderer);
            }
        }
    }
}