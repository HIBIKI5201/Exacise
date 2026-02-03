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
            _duration = 0.5f;
            _toPosition = Vector2.zero;
        }

        public ActorMove(string actorName, float duration, Vector2 toPosition)
        {
            _actorName = actorName;
            _duration = duration;
            _toPosition = toPosition;
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
        private float _duration;
        [SerializeField]
        private Vector2 _toPosition;
    }
}
