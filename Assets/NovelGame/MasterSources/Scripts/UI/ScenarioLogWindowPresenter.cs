using System;
using UnityEngine;
using UnityEngine.UIElements;
using static NovelGame.Master.Scripts.UI.ScenarioLogWindowViewModel;

namespace NovelGame.Master.Scripts.UI
{
    [Serializable]
    public class ScenarioLogWindowPresenter
    {
        public VisualElement CreateView(ScenarioLogWindowViewModel vm)
        {
            VisualElement root = _windowTreeAsset.Instantiate();
            _list = root.Q<ListView>(_listViewName);
            ListInit(_list);

            _vm = vm;

            return root;
        }

        [SerializeField]
        private VisualTreeAsset _windowTreeAsset;
        [SerializeField]
        private VisualTreeAsset _nodeTreeAsset;

        [SerializeField]
        private string _listViewName = "list-view";

        private ScenarioLogWindowViewModel _vm;
        private ListView _list;

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
