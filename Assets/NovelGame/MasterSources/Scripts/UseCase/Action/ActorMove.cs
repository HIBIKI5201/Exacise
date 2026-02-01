using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.UI;
using NovelGame.Master.Scripts.Utility;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Master.Scripts.UseCase
{
    public class ActorMove : IScenarioAction
    {
        public ActorMove()
        {
            _actorName = string.Empty;
            _toPosition = Vector2.zero;
            _duration = 0.5f;
        }

        public ActorMove(string actorName, Vector2 toPosition, float duration)
        {
            _actorName = actorName;
            _toPosition = toPosition;
            _duration = duration;
        }

        public ValueTask ExecuteAsync(ActionRepository repository, IPauseHandler pauseHandler, CancellationToken token = default)
        {
            ActorPresenter actor = repository.ActorRepository.GetActorPresenter(_actorName);
            Vector2 from = actor.transform.position;
            return Tween.Tweening(from,
                p => actor.transform.position = p,
                _toPosition, _duration,
                ph: pauseHandler,
                token: token);
        }

        [SerializeField]
        private string _actorName;
        [SerializeField]
        private Vector2 _toPosition;
        [SerializeField]
        private float _duration;
    }
}
