using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.UseCase;
using NovelGame.Master.Scripts.Utility;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Master.Scripts.UseCase
{
    public class BackGroundCrossFade : IScenarioAction
    {
        public BackGroundCrossFade()
        {
            _imageName = string.Empty;
            _duration = 0.5f;
        }
        public BackGroundCrossFade(string imageName, float duration)
        {
            _imageName = imageName;
            _duration = duration;
        }
        public async ValueTask ExecuteAsync(ActionRepository repository, IPauseHandler pauseHandler, CancellationToken token = default)
        {
            BackGroundAsset asset = repository.BackGroundDataBase[_imageName];
            await repository.BackGroundPresenter.CrossFadeAsync(
                asset.Sprite,
                _duration,
                pauseHandler,
                token);
        }

        [SerializeField]
        private string _imageName;
        [SerializeField]
        private float _duration;
    }
}
