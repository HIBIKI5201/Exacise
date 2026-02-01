using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelGame.Master.Scripts.UI
{
    /// <summary>
    ///     スキップウィンドウUIを管理します。
    /// </summary>
    [Serializable]
    public class SkipWindowPresenter
    {
        public VisualElement CreateView()
        {
            _root = _visualTreeAsset.Instantiate();
            RootStyleInit(_root.style);
            _root.style.visibility = Visibility.Hidden;
            return _root;
        }

        [SerializeField]
        private VisualTreeAsset _visualTreeAsset;

        private VisualElement _root;

        private void RootStyleInit(IStyle style)
        {
            style.position = Position.Absolute;
            style.flexGrow = 1;
            style.width = Length.Percent(100);
            style.height = Length.Percent(100);
        }
    }
}
