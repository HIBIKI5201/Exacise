using NovelGame.Master.Scripts.UseCase;
using NovelGame.Master.Scripts.Utility;
using UnityEngine;

namespace NovelGame.Master.Scripts.Infra
{
    /// <summary>
    ///     シナリオノードを表します。
    /// </summary>
    public class ScenarioNode
    {

        public ScenarioNode(string text, string name,
            bool isWaitForInput,
            IScenarioAction[] actionObjects)
        {
            _text = text;
            _name = name;
            _isWaitForInput = isWaitForInput;
            _actionObject = actionObjects;
        }

        public string Name => _name;
        public string Text => _text;
        public bool IsWaitForInput => _isWaitForInput;
        public IScenarioAction[] ActionObject => _actionObject;

        [SerializeField]
        private string _name = string.Empty;
        [SerializeField, TextArea(3, 10)]
        private string _text = string.Empty;
        [SerializeField]
        private bool _isWaitForInput = true;
        [SerializeReference, SubclassSelector]
        private IScenarioAction[] _actionObject = null;
    }
}
