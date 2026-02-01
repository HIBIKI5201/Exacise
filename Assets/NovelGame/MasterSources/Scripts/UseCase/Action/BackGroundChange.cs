using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.UseCase;
using NovelGame.Master.Scripts.Utility;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Master.Scripts.UseCase
{
    public class BackGroundChange : IScenarioAction
    {
        public BackGroundChange() { }
        public BackGroundChange(string imageName)
        {
            _imageName = imageName;
        }

        public ValueTask ExecuteAsync(ActionRepository repository, IPauseHandler pauseHandler, CancellationToken token = default)
        {
            BackGroundAsset asset = repository.BackGroundDataBase[_imageName];
            repository.BackGroundPresenter.SetFrontSpriteAsync(asset.Sprite);

            return default;
        }

        [SerializeField]
        private string _imageName;
    }
}
