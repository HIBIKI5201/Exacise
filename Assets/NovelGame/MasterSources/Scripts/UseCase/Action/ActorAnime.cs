using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.UI;
using NovelGame.Master.Scripts.Utility;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Master.Scripts.UseCase
{
    public class ActorAnime : IScenarioAction
    {
        public ActorAnime()
        {
            _actorName = string.Empty;
            _clipName = string.Empty;
        }

        public ActorAnime(string actorName, string clipName)
        {
            _actorName = actorName;
            _clipName = clipName;
        }

        public async ValueTask ExecuteAsync(ActionRepository repository, IPauseHandler pauseHandler, CancellationToken token = default)
        {
            ActorPresenter actor = repository.ActorRepository.GetActorPresenter(_actorName);
            await actor.PlayAnimationAsync(_clipName, pauseHandler);
        }

        [SerializeField]
        private string _actorName;
        [SerializeField]
        private string _clipName;
    }
}
