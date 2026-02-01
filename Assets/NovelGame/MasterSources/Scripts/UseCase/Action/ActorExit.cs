using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.UI;
using NovelGame.Master.Scripts.Utility;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Master.Scripts.UseCase
{
    public class ActorExit : IScenarioAction
    {
        public ActorExit()
        {
            _actorName = string.Empty;
            _duration = 0.5f;
        }

        public async ValueTask ExecuteAsync(ActionRepository repository, IPauseHandler pauseHandler, CancellationToken token = default)
        {
            ActorPresenter actor = repository.ActorRepository.GetActorPresenter(_actorName);
            await actor.FadeOut(_duration, pauseHandler, token);
        }

        [SerializeField]
        private string _actorName;
        [SerializeField]
        private float _duration;
    }
}
