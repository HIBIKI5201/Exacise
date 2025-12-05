using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Scripts
{
    public class CharacterAnimator : MonoBehaviour
    {
        public string Name => name;

        public async Task PlayAction(string[] actions, CancellationToken token = default)
        {
            foreach (string action in actions)
            {
                if (string.IsNullOrEmpty(action)) continue;

                string[] inputs = action.Split();
                switch (inputs[0].ToLower())
                {
                    case "fadein":
                        float fadeInDuration = inputs.Length > 1 && float.TryParse(inputs[1], out float inDuration) ? inDuration : 0.5f;
                        await FadeIn(fadeInDuration, token);
                        break;

                    case "fadeout":
                        float fadeOutDuration = inputs.Length > 1 && float.TryParse(inputs[1], out float outDuration) ? outDuration : 0.5f;
                        await FadeOut(fadeOutDuration, token);
                        break;

                    default:
                        await PlayAnimationAsync(inputs[0], token);
                        break;
                }
            }
        }

        public async Task FadeIn(float duration = 0.5f, CancellationToken token = default)
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
                }
            }
            finally
            {
                // 最後にアルファ値を1に設定。
                ChangeAllColorAlpha(1f);
            }
        }

        public async Task FadeOut(float duration = 0.5f, CancellationToken token = default)
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
                }
            }
            finally
            {
                // 最後にアルファ値を0に設定。
                ChangeAllColorAlpha(0f);
            }
        }

        public async Task PlayAnimationAsync(string animation, CancellationToken token = default)
        {
            _animator.Play(animation);

            // アニメーションの長さを取得。
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float animationLength = stateInfo.length;

            try
            {
                await Awaitable.WaitForSecondsAsync(animationLength, token);
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
            foreach (var renderer in _renderers)
            {
                Color color = renderer.color;
                color.a = a;
                renderer.color = color;
            }
        }

#if UNITY_EDITOR
        [SerializeField]
        private float _fadeInDuration = 2f;
        [SerializeField]
        private float _fadeOutDuration = 2f;

        [ContextMenu("FadeIn")]
        private void FadeIn() => _ = FadeIn(_fadeInDuration);
        [ContextMenu("FadeOut")]
        private void FadeOut() => _ = FadeOut(_fadeOutDuration);
#endif
    }
}