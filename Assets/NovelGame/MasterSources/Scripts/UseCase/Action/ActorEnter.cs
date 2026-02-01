using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.UI;
using NovelGame.Master.Scripts.UseCase;
using NovelGame.Master.Scripts.Utility;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Master
{
    public class ActorEnter : IScenarioAction
    {
        public ActorEnter() { }

        public ActorEnter(string actorName, float duration, Vector2 position)
        {
            _actorName = actorName;
            _duration = duration;
            _position = position;
        }

        public ValueTask ExecuteAsync(ActionRepository repository, IPauseHandler pauseHandler, CancellationToken token = default)
        {
            ActorPresenter actor = repository.ActorDataBase[_actorName];
            actor = Object.Instantiate(actor, _position, Quaternion.identity);

            return actor.FadeIn(_duration, pauseHandler, token);
        }

        [SerializeField]
        private string _actorName;
        [SerializeField]
        private float _duration;
        [SerializeField]
        private Vector2 _position;
    }
}
