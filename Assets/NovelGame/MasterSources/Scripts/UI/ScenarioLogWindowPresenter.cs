using System;
using UnityEngine;
using UnityEngine.UIElements;
using static NovelGame.Master.Scripts.UI.ScenarioLogWindowViewModel;

namespace NovelGame.Master.Scripts.UI
{
    [Serializable]
    public class ScenarioLogWindowPresenter
    {
        public VisualElement Root => _root;

        public VisualElement CreateView(ScenarioLogWindowViewModel vm)
        {
            _vm = vm;
            VisualElement root = _windowTreeAsset.Instantiate();
            _root = root;
            RootInit(root);

            _list = root.Q<ListView>(_listViewName);
            ListInit(_list);

            ChangeVisibility(Visibility.Hidden);

            return root;
        }

        public void ChangeVisibility(Visibility visibility)
        {
            _root.style.visibility = visibility;
        }

        [SerializeField]
        private VisualTreeAsset _windowTreeAsset;
        [SerializeField]
        private VisualTreeAsset _nodeTreeAsset;

        [SerializeField]
        private string _listViewName = "list-view";

        private ScenarioLogWindowViewModel _vm;
        private VisualElement _root;
        private ListView _list;

        private void RootInit(VisualElement root)
        {
            root.pickingMode = PickingMode.Ignore;

            IStyle style = root.style;
            style.position = Position.Absolute;
            style.width = Length.Percent(100);
            style.height = Length.Percent(100);
        }

        private void ListInit(ListView list)
        {
            list.itemsSource = _vm.DisplayedNodes;
            list.makeItem = () => _nodeTreeAsset.Instantiate();
            list.bindItem = (element, i) =>
            {
                ScenarioNodeViewModel node = _vm[i];
                element.dataSource = node;
            };
        }
    }
}
