using NovelGame.Master.Scripts.UI;
using UnityEngine;

namespace NovelGame.Master.Scripts.Runner
{
    public class Runner : MonoBehaviour
    {
        [SerializeField]
        private NovelUIPresenter _novelUIPresenter;
        [SerializeField]
        private BackGroundPresenter _bgPresenter;

        private void Start()
        {
            if (_novelUIPresenter == null)
            { _novelUIPresenter = FindAnyObjectByType<NovelUIPresenter>(); }
            if (_novelUIPresenter == null)
            { _bgPresenter = FindAnyObjectByType<BackGroundPresenter>(); }

            MessageWindowViewModel messageWindowVM = ScriptableObject.CreateInstance<MessageWindowViewModel>();
            _novelUIPresenter.BindMessageWindowViewModel(messageWindowVM);
        }
    }
}