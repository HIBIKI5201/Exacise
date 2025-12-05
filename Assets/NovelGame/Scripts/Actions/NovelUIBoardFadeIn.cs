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
        protected override async Task Proccess(MassageWindowPresenter manager, CancellationToken token)
        {
            await manager.FadeInBoard(_duration, token);
        }

        [SerializeField]
        private float _duration = 0.5f;
    }
}