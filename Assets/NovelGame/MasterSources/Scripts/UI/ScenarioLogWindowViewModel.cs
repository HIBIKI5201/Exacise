using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.Presenter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NovelGame.Master.Scripts.UI
{
    public class ScenarioLogWindowViewModel : ScriptableObject
    {
        public event Action<int> OnMoveNext
        {
            add => _player.OnMoveNext += value;
            remove => _player.OnMoveNext -= value;
        }

        public List<ScenarioNodeViewModel> DisplayedNodes => _displayedNodes;
        public ScenarioNodeViewModel this[int index] => _displayedNodes[index];

        public void Bind(ScenarioDataAsset scenario, ScenarioPlayer player)
        {
            _scenario = scenario;
            _player = player;

            _allNodes = new ScenarioNodeViewModel[_scenario.Length];
            for (int i = 0; i < _scenario.Length; i++)
            {
                var node = _scenario[i];
                _allNodes[i] = new ScenarioNodeViewModel(node.Name, node.Text);
            }

            _player.OnMoveNext += UpdateDisplayedNodes;
            _displayedNodes.Clear();
        }

        public void UpdateDisplayedNodes(int currentIndex)
        {
            for (int i = _displayedNodes.Count; i <= currentIndex; i++)
            {
                _displayedNodes.Add(_allNodes[i]);
            }
        }

        public class ScenarioNodeViewModel
        {
            public ScenarioNodeViewModel(string name, string message)
            {
                Name = name;
                Message = message;
            }

            public readonly string Name;
            public readonly string Message;
        }

        private ScenarioNodeViewModel[] _allNodes;
        private readonly List<ScenarioNodeViewModel> _displayedNodes = new();

        private ScenarioDataAsset _scenario;
        private ScenarioPlayer _player;
    }
}

