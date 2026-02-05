using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelGame.Master.Scripts.UI
{
    [Serializable]
    public class MessageWindowPresenter
    {
        public event Action OnButtonClicked;

        public VisualElement Root => _root;
        public bool IsButtonActived
        {
            get => _isButtonActive;
            set => _isButtonActive = value;
        }

        public VisualElement CreateView(MessageWindowViewModel vm)
        {
            _root = _visualTreeAsset.Instantiate();
            RootStyleInit(_root);
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
        private bool _isButtonActive = true;

        private void RootStyleInit(VisualElement root)
        {
            _root.pickingMode = PickingMode.Ignore;

            IStyle style = root.style;
            style.position = Position.Absolute;
            style.width = Length.Percent(100);
            style.height = Length.Percent(100);
        }

        private void ClickButtonInit(Button button)
        {
            button.clicked += OnClickButton;
        }

        private void OnClickButton()
        {
            if(!_isButtonActive) { return; }

            OnButtonClicked?.Invoke();
        }
    }
}
