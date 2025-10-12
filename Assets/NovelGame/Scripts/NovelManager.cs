using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace NovelGame.Scripts
{
    /// <summary>
    ///     ノベルゲームの管理を行います。
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(100)]
    [RequireComponent(typeof(NovelPrinter))]
    public class NovelManager : MonoBehaviour
    {
        [SerializeField]
        private MassageWindowPresenter _massageWindowPresenter;

        [Space]
        [SerializeField]
        private NovelData _novelData;

        private IAsyncEnumerator<byte> _textEnumerator;
        private ValueTask<bool> _textMoveNextTask;

        private CancellationTokenSource _cts;

        private NovelPrinter _printer;

        private void Awake()
        {
            _printer = GetComponent<NovelPrinter>();
        }

        private void Start()
        {
            _textEnumerator = NextText(_novelData, destroyCancellationToken);
            if (_massageWindowPresenter == null)
            {
                Debug.LogError("MassageWindowPresenterがアタッチされていません。", this);
                return;
            }
            _massageWindowPresenter.OnClickButton += HandleClicked;

            destroyCancellationToken.Register(() =>
            {
                _massageWindowPresenter.OnClickButton -= HandleClicked;
                _ = _textEnumerator.DisposeAsync();
            });
        }

        private async void HandleClicked()
        {
            if (_textMoveNextTask == null || _textMoveNextTask.IsCompleted)
            {
                try
                {
                    _textMoveNextTask = _textEnumerator.MoveNextAsync();
                    await _textMoveNextTask;
                }
                catch (TaskCanceledException) { }
            }
            else // 文字送り中にクリックされた場合には、文字送りをスキップする。
            {
                CancelShowing();
            }
        }

        private async IAsyncEnumerator<byte> NextText(NovelData data, CancellationToken token)
        {
            if (data == null)
            {
                Debug.LogError("NovelDataがアタッチされていません。", this);
                yield break;
            }

            int index = 0;

            while (index < _novelData.Length)
            {
                string name = _novelData[index].Name;
                _massageWindowPresenter.SetName(name);

                string text = _novelData[index].Text;

                _cts = new();

                Task textTask = _printer.ShowTextAsync(text, _cts.Token);

                await Task.WhenAll(textTask);

                yield return 0;
                index++;
            }
        }

        private void CancelShowing()
        {
            _cts.Cancel();
        }
    }
}