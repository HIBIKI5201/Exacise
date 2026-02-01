using NovelGame.Master.Scripts.Utility;
using UnityEngine;

namespace NovelGame.Master.Scripts.Infra
{
    /// <summary>
    ///     背景アセットを管理します。
    /// </summary>
    [CreateAssetMenu(fileName = nameof(BackGroundAsset), 
        menuName = InfraContraint.ASSET_PATH + nameof(BackGroundAsset))]
    public sealed class BackGroundAsset : ScriptableObject
    {
        public string BackGroundName => name;
        /// <summary> 背景スプライトを取得します。 </summary>
        public Sprite Sprite => _backgroundSprite;

        [SerializeField]
        private Sprite _backgroundSprite;
    }
}
