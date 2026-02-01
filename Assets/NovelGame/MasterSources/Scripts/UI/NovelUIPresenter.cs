using NovelGame.Master.Scripts.Utility;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelGame.Master.Scripts.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class NovelUIPresenter : MonoBehaviour
    {
        public IPauseHandler PauseHandler => _skipWindowPresenter;

        public void BindSkipButtonClickedEvent(Action action)
        {
            _messageWindowPresenter.OnButtonClicked += action;
        }

        public void BindMessageWindowViewModel(MessageWindowViewModel vm)
        {
            _root.Add(_messageWindowPresenter.CreateView(vm));
        }

        [SerializeField]
        private MessageWindowPresenter _messageWindowPresenter;
        [SerializeField]
        private SkipWindowPresenter _skipWindowPresenter;

        private UIDocument _document;
        private VisualElement _root;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _root = _document.rootVisualElement;

            _root.Add(_skipWindowPresenter.CreateView());
        }
    }
}
