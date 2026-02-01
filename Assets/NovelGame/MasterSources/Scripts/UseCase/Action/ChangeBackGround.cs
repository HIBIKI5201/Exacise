using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.UseCase;
using NovelGame.Master.Scripts.Utility;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Master
{
    public class ChangeBackGround : IScenarioAction
    {
        public ChangeBackGround() { }
        public ChangeBackGround(string imageName)
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
