using NovelGame.Scripts;
using System.Threading;
using System.Threading.Tasks;

namespace NovelGame.Scripts
{
    /// <summary>
    ///     ノベルゲームのアクションを表します。
    /// </summary>
    public interface IAction
    {
        public Task ExcuteAsync(NovelObjectRepository repository, CancellationToken token = default);
    }
}