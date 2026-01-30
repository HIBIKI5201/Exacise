using System.Threading;
using System.Threading.Tasks;

namespace NovelGame.Scripts
{
    /// <summary>
    ///     背景アクションの基底クラスを表します。
    /// </summary>
    public abstract class BackGroundActionBase : IAction
    {
        public async Task ExcuteAsync(NovelObjectRepository repository, IPauseHandler ph, CancellationToken token = default)
        {
            BackGroundUIManager manager = repository.BackGroundUIManager;
            await Proccess(manager, ph, token);
        }
        protected abstract Task Proccess(BackGroundUIManager manager, IPauseHandler ph, CancellationToken token);
    }
}