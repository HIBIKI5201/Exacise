using NovelGame.Master.Scripts.UI;
using NovelGame.Master.Scripts.Utility;
using UnityEngine;

namespace NovelGame.Master.Scripts.Infra
{
    /// <summary>
    ///     アクションに使用するPresenterを管理します。
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ActionRepository),
        menuName = InfraContraint.ASSET_PATH + nameof(ActionRepository))]
    public class ActionRepository
    {
        public ActionRepository(
            NovelUIPresenter novelUIPresenter,
            BackGroundPresenter backGroundPresenter,
            ActorRepository actorRepo,
            BackGroundAssetDataBase backGroundDataBase
            )
        {
            _novelUIPresenter = novelUIPresenter;
            _bgPresenter = backGroundPresenter;
            _actorRepo = actorRepo;
            _backGroundDataBase = backGroundDataBase;
        }

        public NovelUIPresenter NovelUIPresenter => _novelUIPresenter;
        public BackGroundPresenter BackGroundPresenter => _bgPresenter;
        public ActorRepository ActorRepository => _actorRepo;
        public BackGroundAssetDataBase BackGroundDataBase => _backGroundDataBase;

        private readonly NovelUIPresenter _novelUIPresenter;
        private readonly BackGroundPresenter _bgPresenter;
        private readonly ActorRepository _actorRepo;
        private readonly BackGroundAssetDataBase _backGroundDataBase;
    }
}