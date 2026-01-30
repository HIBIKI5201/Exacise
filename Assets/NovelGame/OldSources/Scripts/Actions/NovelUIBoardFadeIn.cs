using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Scripts.Actions
{
    /// <summary>
    ///     ノベルUIのボードをフェードインさせるアクションを表します。
    /// </summary>
    public class NovelUIBoardFadeIn : NovelUIActionBase
    {
        public NovelUIBoardFadeIn(float duration)
        {
            _duration = duration;
        }

        protected override async Task Proccess(MassageWindowPresenter manager, IPauseHandler ph, CancellationToken token)
        {
            await manager.FadeInBoard(_duration, ph, token);
        }

        [SerializeField]
        private float _duration = 0.5f;
    }
}