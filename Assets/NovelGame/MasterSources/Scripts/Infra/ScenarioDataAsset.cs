using NovelGame.Master.Scripts.Utility;
using UnityEngine;

namespace NovelGame.Master.Scripts.Infra
{
    /// <summary>
    ///     ノベルゲームのデータアセットを表します。
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ScenarioDataAsset), 
        menuName = InfraContraint.ASSET_PATH + nameof(ScenarioDataAsset))]
    public class ScenarioDataAsset : ScriptableObject
    {
        public ScenarioNode this[int index] => _scenarioNodes[index];
        public int Length => _scenarioNodes.Length;
        public void SetScenarioNodes(ScenarioNode[] scenarioNodes)
        {
            _scenarioNodes = scenarioNodes;
        }

        [SerializeField]
        private ScenarioNode[] _scenarioNodes;
    }
}
