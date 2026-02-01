using System;
using UnityEngine.UIElements;
using UnityEngine;

namespace NovelGame.Master.Scripts.UI
{
    [Serializable]
    public class MessageWindowPresenter
    {
        public VisualElement CreateView(MessageWindowViewModel vm)
        {
            _root = _visualTreeAsset.Instantiate();
            RootStyleInit(_root.style);
            _root.dataSource = vm;

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
