using NovelGame.Master.Scripts.Utility;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Master.Scripts.UI
{
    /// <summary>
    ///     ノベルゲームのキャラクター表示を管理します。
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class ActorPresenter : MonoBehaviour
    {
        public string Name => name;

        public async ValueTask FadeIn(float duration = 0.5f, IPauseHandler ph = null, CancellationToken token = default)
        {
            if (duration <= 0f)
            {
                ChangeAllColorAlpha(1f);
                return;
            }

            try
            {
                await Tween.Tweening(0, n => ChangeAllColorAlpha(n), 1,
                    d: duration,
                    ph: ph,
                    token: token);
            }
            catch(OperationCanceledException) { }
            finally
            {
                // 最後にアルファ値を1に設定。
                ChangeAllColorAlpha(1f);
            }
        }

        public async ValueTask FadeOut(float duration = 0.5f, IPauseHandler ph = null, CancellationToken token = default)
        {
            if (duration <= 0f)
            {
                ChangeAllColorAlpha(0f);
                return;
            }

            try
            {
                await Tween.Tweening(1, n => ChangeAllColorAlpha(n), 0,
                    d: duration,
                    ph: ph,
                    token: token);
            }
            catch (OperationCanceledException) { }
            finally
            {
                // 最後にアルファ値を0に設定。
                ChangeAllColorAlpha(0f);
            }
        }

        public async ValueTask PlayAnimationAsync(string animation, IPauseHandler ph, CancellationToken token = default)
        {
            _animator.Play(animation);

            // アニメーションの長さを取得。
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float animationLength = stateInfo.length;

            float elapsed = 0;
            try
            {
                while (elapsed <= animationLength)
                {
                    await Awaitable.NextFrameAsync(token);
                    elapsed += Time.deltaTime;

                    if (ph != null && ph.IsPaused)
                    {
                        _animator.speed = 0;
                        await ph.WaitResumeAsync(token);
                        _animator.speed = 1;
                    }
                }
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

            ChangeAllColorAlpha(0); // 非表示。
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
            foreach (SpriteRenderer renderer in _renderers)
            {
                action(renderer);
            }
        }
    }
}
