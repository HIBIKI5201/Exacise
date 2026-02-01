using NovelGame.Master.Scripts.Utility;
using UnityEngine;

namespace NovelGame.Master.Scripts.Infra
{
    /// <summary>
    ///     背景データベースアセットを管理します。
    /// </summary>
    [CreateAssetMenu(fileName = nameof(BackGroundAssetDataBase), 
        menuName = InfraContraint.ASSET_PATH + nameof(BackGroundAssetDataBase))]
    public class BackGroundAssetDataBase : ScriptableObject
    {
        public BackGroundAsset this[string bgName]
        {
            get
            {
                foreach (var bgAsset in _backgroundAssets)
                {
                    if (bgAsset.BackGroundName == bgName)
                    {
                        return bgAsset;
                    }
                }

                Debug.LogWarning($"背景アセットが見つかりません: {bgName}");
                return null;
            }
        }

        /// <summary>
        ///     背景スプライトを名前で取得します。
        /// </summary>
        /// <param name="bgName"></param>
        /// <param name="bgAsset"></param>
        /// <returns></returns>
        public bool TryGetValue(string bgName, out Sprite bgAsset)
        {
            foreach (var asset in _backgroundAssets)
            {
                if (asset.BackGroundName == bgName)
                {
                    bgAsset = asset.Sprite;
                    return true;
                }
            }

            bgAsset = null;
            return false;
        }

        [SerializeField]
        private BackGroundAsset[] _backgroundAssets;

        private void OnValidate()
        {
            for (int i = 0; i < _backgroundAssets.Length; i++)
            {
                BackGroundAsset bgAsset = _backgroundAssets[i];

                if (bgAsset == null)
                {
                    Debug.LogWarning($"{i}番目の背景アセットが設定されていません。");
                    continue;
                }

                if (bgAsset.Sprite == null)
                {
                    Debug.LogWarning($"背景アセットのスプライトが設定されていません: {bgAsset.BackGroundName}");
                    continue;
                }
            }
        }
    }
}