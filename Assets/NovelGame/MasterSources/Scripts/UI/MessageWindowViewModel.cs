using NovelGame.Master.Scripts.Utility;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Master.Scripts.UI
{
    public class MessageWindowViewModel : ScriptableObject
    {
        public void Init(IPauseHandler ph)
        {
            _ph = ph;
        }

        public ValueTask SetTextAsync(string characterName, string messageText)
        {
            _characterName = characterName;
            return TextCaptionAsync(messageText);
        }

        private IPauseHandler _ph;

        [SerializeField] private string _characterName = string.Empty;
        [SerializeField] private string _messageText = string.Empty;

        private async ValueTask TextCaptionAsync(string text, CancellationToken token = default)
        {
            // 文字を徐々に表示する。
            float showLength = 0;
            while (showLength < text.Length)
            {
                showLength += 1 * Time.deltaTime;

                // 次に表示する文字数を計算。
                int nextShowLength = Mathf.Min(
                        (int)showLength, //速度に応じた数。
                        text.Length); // 最大文字数。

                _messageText = text[..nextShowLength];

                try // 1フレーム待機。
                {
                    await Awaitable.NextFrameAsync(token);

                    // ポーズ中は停止する。
                    await _ph.WaitResumeAsync(token);
                }
                catch (OperationCanceledException)
                {
                    // 文字送りがスキップされた場合にはループを抜ける。
                    break;
                }
            }

            // 最後に全文字を表示しておく。
            _messageText = text;
        }
    }
}