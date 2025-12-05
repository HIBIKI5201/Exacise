using NovelGame.Scripts;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Scripts
{
    /// <summary>
    ///     背景をクロスフェードで変更するアクションを表します。
    /// </summary>
    public class BackGroundCrossFade : BackGroundActionBase
    {
        [SerializeField]
        private string _assetName;
        [SerializeField]
        private float _duration = 1.0f;

        protected override Task Proccess(BackGroundUIManager manager, CancellationToken token)
        {
            Sprite newSprite = manager.Database[_assetName];
            return manager.FadeInSpriteAsync(newSprite, _duration, token);
        }
    }
}