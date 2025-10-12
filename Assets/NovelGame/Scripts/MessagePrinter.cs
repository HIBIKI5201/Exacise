using NovelGame.Scripts;
using System.Collections;
using System.Text;
using UnityEngine;

public class MessagePrinter : MonoBehaviour
{
    [SerializeField]
    private MassageWindowPresenter _massageWindowPresenter;

    [SerializeField]
    private NovelSettings _settings;

    private bool _isCanceled = false;
    private string _text;

    public IEnumerator ShowText(string text)
    {
        _text = text;

        // 文字を徐々に表示する。
        float showLength = 0;
        while (showLength < text.Length)
        {
            showLength += _settings.ShowSpeed * Time.deltaTime;

            // 次に表示する文字数を計算。
            int nextShowLength = Mathf.Min(
                    (int)showLength, //速度に応じた数。
                    text.Length); // 最大文字数。

            _massageWindowPresenter.SetMassage(text);

            yield return null;

            if (_isCanceled)
            {
                _isCanceled = false;
                break;
            }
        }

        // 最後に全文字を表示しておく。
        _massageWindowPresenter.SetMassage(text);
        _isCanceled = false;
    }

    public void CancelShowing()
    {
        _massageWindowPresenter.SetMassage(_text);
        _isCanceled = true;
    }
}
