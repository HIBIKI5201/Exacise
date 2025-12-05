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
        [SerializeField]
        private Image _front;
        [SerializeField]
        private Image _back;

        public async Task PlayBackGroundActionAsync(string[] actions, CancellationToken token = default)
        {
            foreach (string action in actions)
            {
                if (string.IsNullOrEmpty(action)) return;
                string[] inputs = action.Split();
                switch (inputs[0].ToLower())
                {
                    case "changebackground":
                        float fadeInDuration = inputs.Length > 1 && float.TryParse(inputs[2], out float inDuration) ? inDuration : 0.5f;
                        await FadeInSpriteAsync(inputs[1], fadeInDuration, token);
                        break;
                    default:
                        break;
                }
            }

        }

        public void SetFrontSpriteAsync(Sprite sprite)
        {
            _front.sprite = sprite;
        }

        public async Task FadeInSpriteAsync(string assetName, float duration, CancellationToken token = default)
        {
            Sprite sprite = Resources.Load<Sprite>(assetName);
            await FadeInSpriteAsync(sprite, duration, token);
        }

        public async Task FadeInSpriteAsync(Sprite sprite, float duration, CancellationToken token = default)
        {
            _back.sprite = sprite;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                Color color = _front.color;
                color.a = 1f - Mathf.Clamp01(elapsed / duration);
                _front.color = color;

                await Awaitable.NextFrameAsync(token);
                elapsed += Time.deltaTime;
            }
            _front.sprite = sprite;
            _front.color = Color.white;
        }
    }
}