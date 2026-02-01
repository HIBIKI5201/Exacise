using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.Presenter;
using NovelGame.Master.Scripts.UI;
using UnityEngine;

namespace NovelGame.Master.Scripts.Runner
{
    public class Runner : MonoBehaviour
    {
        [SerializeField]
        private ScenarioDataAsset _scenarioAsset;
        [SerializeField]
        private ActorAssetDataBase _actorDataBase;

        [SerializeField]
        private NovelUIPresenter _novelUIPresenter;
        [SerializeField]
        private BackGroundPresenter _bgPresenter;

        private void Start()
        {
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

            if (_novelUIPresenter == null)
            { _novelUIPresenter = FindAnyObjectByType<NovelUIPresenter>(); }
            if (_novelUIPresenter == null)
            { _bgPresenter = FindAnyObjectByType<BackGroundPresenter>(); }

            MessageWindowViewModel messageWindowVM = ScriptableObject.CreateInstance<MessageWindowViewModel>();
            _novelUIPresenter.BindMessageWindowViewModel(messageWindowVM);

            ScenarioPlayer scenarioPlayer = new ScenarioPlayer(
                asset: _scenarioAsset,
                repo: new ActionRepository(_novelUIPresenter, _bgPresenter, _actorDataBase),
                pause: null);
        }
    }
}