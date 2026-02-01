using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelGame.Master
{
    [Serializable]
    public class ButtonListPresenter
    {
        public event Action OnLogButtonClicked;
        public event Action OnSkipButtonClicked;

        public void Bind(VisualElement root)
        {
            root = root.Q(_buttonListRootName);

            Button log = root.Q<Button>(_logButtonName);
            Button skip = root.Q<Button>(_skipButtonName);

            log.clicked += OnLogButtonClick;
            skip.clicked += OnSkipButtonClick;
        }

        [SerializeField]
        private string _buttonListRootName = "button-list";
        [SerializeField]
        private string _logButtonName = "log";
        [SerializeField]
        private string _skipButtonName = "skip";

        private void OnLogButtonClick() => OnLogButtonClicked?.Invoke();
        private void OnSkipButtonClick() => OnSkipButtonClicked?.Invoke();
    }
}
