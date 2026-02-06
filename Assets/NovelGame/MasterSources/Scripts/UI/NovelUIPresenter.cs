using NovelGame.Master.Scripts.Utility;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelGame.Master.Scripts.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class NovelUIPresenter : MonoBehaviour
    {
        public event Action OnSkipRequested
        {
            add => _skipWindowPresenter.OnSkipRequested += value;
            remove => _skipWindowPresenter.OnSkipRequested -= value;
        }

        public event Action<int> OnClickedChoiceButton
        {
            add => _choiceButtonPresenter.OnClicked += value;
            remove =>_choiceButtonPresenter.OnClicked -= value;
        }

        public IPauseHandler PauseHandler => _pauseManager;

        public void BindSkipButtonClickedEvent(Action action)
        {
            _messageWindowPresenter.OnButtonClicked += action;
        }

        public void SetNovelButtonActive(bool active)
        {
            _messageWindowPresenter.IsButtonActived = active;
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
            _choiceButtonPresenter.Root.BringToFront();
            _scenarioLogWindowPresenter.Root.BringToFront();
            _skipWindowPresenter.Root.BringToFront();
        }

        public void CreateScenarioLogWindow(ScenarioLogWindowViewModel vm)
        {
            _root.Add(_scenarioLogWindowPresenter.CreateView(vm));
            _buttonListPresenter.OnLogButtonClicked +=
                () => _scenarioLogWindowPresenter.ChangeVisibility(Visibility.Visible);
        }

        public void ShowButton(string text, int lineIndex, Vector2Int pos)
        {
            _choiceButtonPresenter.PopButton(text, lineIndex, pos);
        }


        [SerializeField]
        private MessageWindowPresenter _messageWindowPresenter;
        [SerializeField]
        private SkipWindowPresenter _skipWindowPresenter;
        [SerializeField]
        private ScenarioLogWindowPresenter _scenarioLogWindowPresenter;
        [SerializeField]
        private ButtonListPresenter _buttonListPresenter;
        [SerializeField]
        private ChoiceButtonPresenter _choiceButtonPresenter;

        private PauseManager _pauseManager;
        private UIDocument _document;
        private VisualElement _root;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _root = _document.rootVisualElement;

            _pauseManager = new(
                _skipWindowPresenter,
                _scenarioLogWindowPresenter);
            _root.Add(_choiceButtonPresenter.Create());

            _choiceButtonPresenter.OnClicked += n => _messageWindowPresenter.IsButtonActived = true;
        }
    }
}
