using NovelGame.Master.Scripts.Utility;
using UnityEngine;

namespace NovelGame.Master.Scripts.Infra
{
    /// <summary>
    /// ノベルゲームのデータアセットを表します。
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ScenarioDataAsset), 
        menuName = InfraContraint.ASSET_PATH + nameof(ScenarioDataAsset))]
    public class ScenarioDataAsset : ScriptableObject
    {

    }
}
