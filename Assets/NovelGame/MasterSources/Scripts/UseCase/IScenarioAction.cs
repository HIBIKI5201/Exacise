using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.Utility;
using System.Threading;
using System.Threading.Tasks;

namespace NovelGame.Master.Scripts.UseCase
{
    /// <summary>
    ///     ノベルゲームのアクションを表します。
    /// </summary>
    public interface IScenarioAction
    {
        public ValueTask ExecuteAsync(ActionRepository repository,
            IPauseHandler pauseHandler,
            CancellationToken token = default);
    }
}