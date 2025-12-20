using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

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
        [SerializeField]
        private BackGroundUIManager _backGroundUIManager;

        [Space]
        [SerializeField]
        private NovelData _novelData;

        private NovelObjectRepository _repository;
        private IAsyncEnumerator<byte> _textEnumerator;
        private ValueTask<bool> _textMoveNextTask;

        private CancellationTokenSource _taskCts;

        private NovelPrinter _printer;

        private void Awake()
        {
            _printer = GetComponent<NovelPrinter>();
        }

        private void Start()
        {
            CharacterAnimator[] characters = FindObjectsByType<CharacterAnimator>(FindObjectsSortMode.None);
            _repository = new(_backGroundUIManager, characters, _massageWindowPresenter);

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
                NovelData.TextData textData = _novelData[index];
                string name = textData.Name;
                _massageWindowPresenter.SetName(name);
                IPauseHandler pauseHandler = _massageWindowPresenter.SkipDialogManager;

                string text = textData.Text;

                _taskCts = new();
                CancellationToken tcn = _taskCts?.Token ?? default;

                IAction[] actions = textData.ActionObject;
                Task[] actionTasks = new Task[actions.Length + 1];
                actionTasks[0] = _printer.ShowTextAsync(text, pauseHandler, tcn);

                for (int i = 0; i < actions.Length; i++) // 事前のタスクの次から入れる。
                { actionTasks[i + 1] = actions[i]?.ExcuteAsync(_repository, pauseHandler, tcn) ?? Task.CompletedTask; }

                try
                {
                    await Task.WhenAll(actionTasks);
                }
                catch (OperationCanceledException) { }
                finally
                {
                    _taskCts.Dispose();
                    _taskCts = null;
                }

                // ユーザー入力を待機。
                if (textData.IsWaitForInput) { yield return 0; }
                index++;
            }
        }

        private void CancelShowing()
        {
            _taskCts.Cancel();
        }
    }
}