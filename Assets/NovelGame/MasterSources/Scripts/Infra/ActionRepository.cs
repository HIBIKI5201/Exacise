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
        public ActionRepository()
        {

        }

        public BackGroundPresenter BackGroundPresenter => _bgPresenter;

        private readonly BackGroundPresenter _bgPresenter;
    }
}