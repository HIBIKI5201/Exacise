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
        public bool IsPaused=> _isPaused;

        public VisualElement CreateView()
        {
            _root = _visualTreeAsset.Instantiate();
            RootStyleInit(_root.style);

            _skipRoot = _root.Q<VisualElement>(_skipRootName);
            Button skipButton = _root.Q<Button>(_skipButtonName);
            SkipButtonInit(skipButton);

            ChangeVisibility(Visibility.Hidden);

            return _root;
        }

        [SerializeField]
        private VisualTreeAsset _visualTreeAsset;

        [SerializeField]
        private string _skipRootName = "skip-root";
        [SerializeField]
        private string _skipButtonName = "skip-button";

        private VisualElement _root;
        private VisualElement _skipRoot;

        private bool _isPaused = false;

        private void RootStyleInit(IStyle style)
        {
            style.position = Position.Absolute;
            style.flexGrow = 1;
            style.width = Length.Percent(100);
            style.height = Length.Percent(100);
        }

        private void SkipButtonInit(Button button)
        {
            button.clicked += OnButtonClicked;
        }

        private void OnButtonClicked()
        {
            ChangeVisibility(Visibility.Visible);
        }

        private void ChangeVisibility(Visibility visibility)
        {
            _skipRoot.style.visibility = visibility;
            _isPaused = visibility == Visibility.Visible;
        }
    }
}
