using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.Presenter;
using NovelGame.Master.Scripts.UI;
using UnityEngine;

namespace NovelGame.Master.Scripts.Runner
{
    public class Runner : MonoBehaviour
    {
        [SerializeField]
        private NovelSetting _novelSetting;
        [SerializeField]
        private ScenarioDataAsset _scenarioAsset;
        [SerializeField]
        private ActorAssetDataBase _actorDataBase;
        [SerializeField]
        private BackGroundAssetDataBase _bgDataBase;

        [Space]

        [SerializeField]
        private NovelUIPresenter _novelUIPresenter;
        [SerializeField]
        private BackGroundPresenter _bgPresenter;

        private ScenarioPlayer _player;

        private void Start()
        {
            if (_novelSetting == null)
            {
                Debug.LogError("NovelSettingが設定されていません。");
                return;
            }

            if (_scenarioAsset == null)
            {
                Debug.LogError("ScenarioDataAssetが設定されていません。");
                return;
            }
            if (_actorDataBase == null)
            {
                Debug.LogError("ActorAssetDataBaseが設定されていません。");
                return;
            }
            if (_bgDataBase == null)
            {
                Debug.LogError("BackGroundAssetDataBaseが設定されていません。");
                return;
            }

            if (_novelUIPresenter == null)
            { _novelUIPresenter = FindAnyObjectByType<NovelUIPresenter>(); }
            if (_novelUIPresenter == null)
            { _bgPresenter = FindAnyObjectByType<BackGroundPresenter>(); }

            MessageWindowViewModel messageWindowVM = ScriptableObject.CreateInstance<MessageWindowViewModel>();
            messageWindowVM.Init(_novelSetting, _novelUIPresenter.PauseHandler);
            _novelUIPresenter.BindMessageWindowViewModel(messageWindowVM);

            ActionRepository repo = new ActionRepository(
                novelUIPresenter: _novelUIPresenter,
                backGroundPresenter: _bgPresenter,
                actorDataBase: _actorDataBase,
                backGroundDataBase: _bgDataBase);

            _player = new ScenarioPlayer(
                asset: _scenarioAsset,
                repo: repo,
                messageWindowViewModel: messageWindowVM,
                ph: _novelUIPresenter.PauseHandler);

            _novelUIPresenter.BindSkipButtonClickedEvent(NextNode);

        }

        private async void NextNode()
        {
            bool result = await _player.MoveNextAsync();
            if (!result)
            {
                NovelEnd();
            }
        }

        private void NovelEnd()
        {
            Debug.Log("ノベルゲーム終了");
        }
    }
}