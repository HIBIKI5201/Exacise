using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.Utility;
using System.Threading;
using System.Threading.Tasks;

namespace NovelGame.Master.Scripts.UseCase
{
    /// <summary>
    ///     クリックによるMoveNextの反応を無効にする。
    /// </summary>
    public class DisableClick : IScenarioAction
    {
        public DisableClick() { }

        public ValueTask ExecuteAsync(ActionRepository repository, IPauseHandler pauseHandler, CancellationToken token = default)
        {
            repository.NovelUIPresenter.SetNovelButtonActive(false);

            return default;
        }
    }
}
