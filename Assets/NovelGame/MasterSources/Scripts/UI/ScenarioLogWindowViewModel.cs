using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.Presenter;
using UnityEngine;

namespace NovelGame.Master.Scripts.UI
{
    public class ScenarioLogWindowViewModel : ScriptableObject
    {
        public void Bind(ScenarioDataAsset scenario,
            ScenarioPlayer player)
        {
            _scenario = scenario;
            _player = player;

            _scenarioNodeViewModels = new ScenarioNodeViewModel[_scenario.Length];
            for (int i = 0; i < _scenario.Length; i++)
            {
                ScenarioNode node = _scenario[i];
                _scenarioNodeViewModels[i] = new(node.Name, node.Text);
            }
        }

        public ScenarioNodeViewModel this[int index] => _scenarioNodeViewModels[index];
        public ScenarioNodeViewModel[] DisplayedNodes => _scenarioNodeViewModels[.._player.CurrentIndex];

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

        private ScenarioNodeViewModel[] _scenarioNodeViewModels;

        private ScenarioDataAsset _scenario;
        private ScenarioPlayer _player;
    }
}
