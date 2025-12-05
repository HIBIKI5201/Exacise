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
        protected override async Task Proccess(MassageWindowPresenter manager, CancellationToken token)
        {
            await manager.FadeOutBoard(_duration, token);
        }

        [SerializeField]
        private float _duration = 0.5f;
    }
}