using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.Utility;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Master.Scripts.UseCase
{
    /// <summary>
    ///     選択肢ボタンを出現させる。
    /// </summary>
    public class ShowButton : IScenarioAction
    {
        public ShowButton()
        {
            _buttonText = string.Empty;
            _jumpLine = 0;
            _pos = default;
        }

        public ShowButton(string buttonText, int jumpLine, Vector2Int pos)
        {
            const int MAX_LENGTH = 10;

            if (MAX_LENGTH < buttonText.Length)
            {
                throw new Exception($"ボタンテキストの最大文字数{MAX_LENGTH}を超えています");
            }

            _buttonText = buttonText;
            _jumpLine = jumpLine;
            _pos = pos;
        }

        public ValueTask ExecuteAsync(ActionRepository repository, IPauseHandler pauseHandler, CancellationToken token = default)
        {
            repository.NovelUIPresenter.ShowButton(_buttonText, _jumpLine, _pos);
            return default;
        }

        [SerializeField]
        private string _buttonText;
        [SerializeField]
        private int _jumpLine;
        [SerializeField]
        private Vector2Int _pos;

    }
}
