using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

        private Dictionary<string, CharacterAnimator> _characterAnimators = new();

        private void Awake()
        {
            _printer = GetComponent<NovelPrinter>();
        }

        private void Start()
        {
            CharacterAnimator[] characters = FindObjectsByType<CharacterAnimator>(FindObjectsSortMode.None);
            Array.ForEach(characters,
                character =>
                {
                    _characterAnimators.TryAdd(character.Name, character);
                });

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
                CancellationToken tkn = _cts?.Token ?? default;

                Task textTask = _printer.ShowTextAsync(text, tkn);
                Task characterTask = PlayCharacterActionAsync(name, _novelData[index].Action, tkn);
                Task uiTask = PlayUIActionAsync(name, _novelData[index].Action, tkn);

                try
                {
                    await Task.WhenAll(textTask, characterTask, uiTask);
                }
                catch (OperationCanceledException) { }
                finally
                {
                    _cts.Dispose();
                    _cts = null;
                }

                yield return 0;
                index++;
            }
        }

        private void CancelShowing()
        {
            _cts.Cancel();
        }

        private Task PlayCharacterActionAsync(string name, string action, CancellationToken token)
        {
            if (_characterAnimators.TryGetValue(name, out CharacterAnimator animator))
            {
                return animator.PlayAction(action, token);
            }
            return Task.CompletedTask;
        }

        private Task PlayUIActionAsync(string name, string action, CancellationToken token)
        {
            // 名前が無ければUIアクションを実行する。
            if (!string.IsNullOrEmpty(name)) return Task.CompletedTask;

            string[] inputs = action.Split();

            switch (inputs[0].ToLower())
            {
                case "fadein":
                    float fadeInDuration = inputs.Length > 1 && float.TryParse(inputs[1], out float inDuration) ? inDuration : 0.5f;
                    return _massageWindowPresenter.FadeInBoard(fadeInDuration, token);
                case "fadeout":
                    float fadeOutDuration = inputs.Length > 1 && float.TryParse(inputs[1], out float outDuration) ? outDuration : 0.5f;
                    return _massageWindowPresenter.FadeOutBoard(fadeOutDuration, token);
                default:
                    return Task.CompletedTask;
            }
        }
    }
}