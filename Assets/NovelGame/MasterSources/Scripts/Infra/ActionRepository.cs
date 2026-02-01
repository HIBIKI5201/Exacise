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
            ActorAssetDataBase actorDataBase,
            BackGroundAssetDataBase backGroundDataBase
            )
        {
            _novelUIPresenter = novelUIPresenter;
            _bgPresenter = backGroundPresenter;
            _actorDataBase = actorDataBase;
            _backGroundDataBase = backGroundDataBase;
        }

        public NovelUIPresenter NovelUIPresenter => _novelUIPresenter;
        public BackGroundPresenter BackGroundPresenter => _bgPresenter;
        public ActorAssetDataBase ActorDataBase => _actorDataBase;
        public BackGroundAssetDataBase BackGroundDataBase => _backGroundDataBase;

        private readonly NovelUIPresenter _novelUIPresenter;
        private readonly BackGroundPresenter _bgPresenter;
        private readonly ActorAssetDataBase _actorDataBase;
        private readonly BackGroundAssetDataBase _backGroundDataBase;
    }
}