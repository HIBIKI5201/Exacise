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
        protected override Task Proccess(BackGroundUIManager manager, IPauseHandler ph, CancellationToken token)
        {
            Sprite newSprite = manager.Database[_assetName];
            return manager.CrossFadeAsync(newSprite, _duration, ph, token);
        }

        public BackGroundCrossFade(string assetName, float duration)
        {
            _assetName = assetName;
            _duration = duration;
        }

        [SerializeField]
        private string _assetName;
        [SerializeField]
        private float _duration = 1.0f;
    }
}