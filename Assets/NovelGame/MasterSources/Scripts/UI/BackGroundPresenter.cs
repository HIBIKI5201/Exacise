using NovelGame.Master.Scripts.Utility;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NovelGame.Master.Scripts.UI
{
    /// <summary>
    ///     ノベルゲームの背景UIを管理します。
    /// </summary>
    public class BackGroundPresenter : MonoBehaviour
    {
        [SerializeField] private Image _front;
        [SerializeField] private Image _back;

        public void SetFrontSpriteAsync(Sprite sprite)
        {
            _front.sprite = sprite;
        }

        public async ValueTask CrossFadeAsync(Sprite sprite, float duration, IPauseHandler ph, CancellationToken token = default)
        {
            _back.sprite = sprite;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                Color color = _front.color;
                color.a = 1f - Mathf.Clamp01(elapsed / duration);
                _front.color = color;

                try
                {
                    await Awaitable.NextFrameAsync(token);
                    await ph.WaitResumeAsync(token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                elapsed += Time.deltaTime;
            }

            _front.sprite = sprite;
            _front.color = Color.white;
        }
    }
}
