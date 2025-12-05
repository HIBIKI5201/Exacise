using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NovelGame.Scripts
{
    /// <summary>
    ///     ノベルゲームの背景UIを管理します。
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(50)]
    public class BackGroundUIManager : MonoBehaviour
    {
        public BackGroundDatabase Database => _database;

        [SerializeField]
        private Image _front;
        [SerializeField]
        private Image _back;

        [SerializeField]
        private BackGroundDatabase _database;

        public void SetFrontSpriteAsync(Sprite sprite)
        {
            _front.sprite = sprite;
        }

        public async Task CrossFadeAsync(Sprite sprite, float duration, CancellationToken token = default)
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