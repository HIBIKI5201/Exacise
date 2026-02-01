using System;
using UnityEngine.UIElements;
using UnityEngine;
using NovelGame.Master.Scripts.Utility;

namespace NovelGame.Master.Scripts.UI
{
    [Serializable]
    public class MessageWindowPresenter
    {
        public event Action OnButtonClicked;

        public VisualElement CreateView(MessageWindowViewModel vm)
        {
            _root = _visualTreeAsset.Instantiate();
            RootStyleInit(_root.style);
            _root.dataSource = vm;

            _clickButton = _root.Q<Button>(ClickButtonName);
            ClickButtonInit(_clickButton);

            return _root;
        }

        [SerializeField]
        private VisualTreeAsset _visualTreeAsset;

        [SerializeField]
        private string ClickButtonName = "click-button";

        private VisualElement _root;
        private Button _clickButton;

        private void RootStyleInit(IStyle style)
        {
            style.position = Position.Absolute;
            style.flexGrow = 1;
            style.width = Length.Percent(100);
            style.height = Length.Percent(100);
        }

        private void ClickButtonInit(Button button)
        {
            button.clicked += OnClickButton;
        }

        private void OnClickButton()
        {
            OnButtonClicked?.Invoke();
        }
    }
}
