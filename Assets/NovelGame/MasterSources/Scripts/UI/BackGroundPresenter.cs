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

            Color origin = _front.color;
            Color from = new(origin.r, origin.g, origin.b, 1);
            Color to = new(origin.r, origin.g, origin.b, 0);

            try
            {
                await Tween.Tweening(from, c => _front.color = c, to,
                    d: duration,
                    ph: ph,
                    token: token);
            }
            catch (OperationCanceledException) { }
            finally
            {
                _front.sprite = sprite;
                _front.color = origin;
            }
        }
    }
}
