using NovelGame.Master.Scripts.Utility;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelGame.Master.Scripts.UI
{
    /// <summary>
    ///     スキップウィンドウUIを管理します。
    /// </summary>
    [Serializable]
    public class SkipWindowPresenter : IPauseHandler
    {
        public event Action OnSkipRequested;

        public bool IsPaused => _isPaused;

        public VisualElement CreateView()
        {
            _root = _visualTreeAsset.Instantiate();
            RootStyleInit(_root);

            _skipRoot = _root.Q<VisualElement>(_skipRootName);
            Button skipButton = _root.Q<Button>(_skipButtonName);
            SkipButtonInit(skipButton);

            Button skipExecuteButton = _skipRoot.Q<Button>(_skipExecuteButtonName);
            Button skipCancelButton = _skipRoot.Q<Button>(_skipCancelButtonName);
            WindowButtonInit(skipExecuteButton, skipCancelButton);

            ChangeVisibility(Visibility.Hidden);

            return _root;
        }

        [SerializeField]
        private VisualTreeAsset _visualTreeAsset;

        [SerializeField]
        private string _skipRootName = "skip-root";
        [SerializeField]
        private string _skipButtonName = "skip-button";
        [SerializeField]
        private string _skipExecuteButtonName = "skip";
        [SerializeField]
        private string _skipCancelButtonName = "cancel";

        private VisualElement _root;
        private VisualElement _skipRoot;

        private bool _isPaused = false;

        private void RootStyleInit(VisualElement root)
        {
            root.pickingMode = PickingMode.Ignore;

            IStyle style = root.style;
            style.position = Position.Absolute;
            style.flexGrow = 1;
            style.width = Length.Percent(100);
            style.height = Length.Percent(100);
        }

        private void SkipButtonInit(Button button)
        {
            button.clicked += OnButtonClicked;
        }

        private void WindowButtonInit(Button skip, Button cancel)
        {
            skip.clicked += OnSkipClicked;
            cancel.clicked += OnCancelClicked;
        }

        private void OnButtonClicked()
        {
            ChangeVisibility(Visibility.Visible);
        }

        private void OnSkipClicked()
        {
            OnSkipRequested?.Invoke();
        }

        private void OnCancelClicked()
        {
            ChangeVisibility(Visibility.Hidden);
        }

        private void ChangeVisibility(Visibility visibility)
        {
            _skipRoot.style.visibility = visibility;
            _isPaused = visibility == Visibility.Visible;
        }
    }
}
