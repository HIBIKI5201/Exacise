using System.Threading;
using System.Threading.Tasks;

namespace NovelGame.Scripts
{
    /// <summary>
    ///     ノベルゲームのUIアクションの基底クラスを表します。
    /// </summary>
    public abstract class NovelUIActionBase : IAction
    {
        public async Task ExcuteAsync(NovelObjectRepository repository, CancellationToken token = default)
        {
            await Proccess(repository.MassageWindowPresenter, token);
        }

        protected abstract Task Proccess(MassageWindowPresenter manager, CancellationToken token);
    }
}