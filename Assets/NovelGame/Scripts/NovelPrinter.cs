using NovelGame.Scripts;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NovelPrinter : MonoBehaviour
{
    [SerializeField]
    private MassageWindowPresenter _massageWindowPresenter;

    [SerializeField]
    private NovelSettings _settings;

    public async Task ShowTextAsync(string text, CancellationToken token)
    {
        // 文字を徐々に表示する。
        float showLength = 0;
        while (showLength < text.Length)
        {
            showLength += _settings.ShowSpeed * Time.deltaTime;

            // 次に表示する文字数を計算。
            int nextShowLength = Mathf.Min(
                    (int)showLength, //速度に応じた数。
                    text.Length); // 最大文字数。

            _massageWindowPresenter.SetMassage(text[..nextShowLength]);

            try // 1フレーム待機。
            {
                await Awaitable.NextFrameAsync(token);
            }
            catch (OperationCanceledException)
            {
                // 文字送りがスキップされた場合にはループを抜ける。
                break;
            }
        }

        // 最後に全文字を表示しておく。
        _massageWindowPresenter.SetMassage(text);
    }
}
