using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Scripts.Actions
{
    /// <summary>
    ///     ノベルUIのボードをフェードアウトさせるアクションを表します。
    /// </summary>
    public class NovelUIBoardFadeOut : NovelUIActionBase
    {
        public NovelUIBoardFadeOut(float duration)
        {
            _duration = duration;
        }

        protected override async Task Proccess(MassageWindowPresenter manager, IPauseHandler ph, CancellationToken token)
        {
            await manager.FadeOutBoard(_duration, ph, token);
        }

        [SerializeField]
        private float _duration = 0.5f;
    }
}