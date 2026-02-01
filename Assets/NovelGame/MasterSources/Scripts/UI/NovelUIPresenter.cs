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

        public void CreateMessageWindow(MessageWindowViewModel vm)
        {
            VisualElement visualElement = _messageWindowPresenter.CreateView(vm);
            _root.Add(visualElement);

            _buttonListPresenter.Bind(visualElement);
        }

        public void CreateSkipWindow()
        {
            _root.Add(_skipWindowPresenter.CreateView());
            _buttonListPresenter.OnSkipButtonClicked +=
                () => _skipWindowPresenter.ChangeVisibility(Visibility.Visible);
        }

        public void Sort()
        {
            _messageWindowPresenter.Root.BringToFront();
            _scenarioLogWindowPresenter.Root.BringToFront();
            _skipWindowPresenter.Root.BringToFront();
        }

        public void CreateScenarioLogWindow(ScenarioLogWindowViewModel vm)
        {
            _root.Add(_scenarioLogWindowPresenter.CreateView(vm));
            _buttonListPresenter.OnLogButtonClicked +=
                () => _scenarioLogWindowPresenter.ChangeVisibility(Visibility.Visible);
        }


        [SerializeField]
        private MessageWindowPresenter _messageWindowPresenter;
        [SerializeField]
        private SkipWindowPresenter _skipWindowPresenter;
        [SerializeField]
        private ScenarioLogWindowPresenter _scenarioLogWindowPresenter;
        [SerializeField]
        private ButtonListPresenter _buttonListPresenter;

        private UIDocument _document;
        private VisualElement _root;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _root = _document.rootVisualElement;
        }
    }
}
