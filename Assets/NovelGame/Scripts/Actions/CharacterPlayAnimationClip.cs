using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Scripts
{
    /// <summary>
    ///     キャラクターにアニメーションクリップを再生させるアクションを表します。
    /// </summary>

    public class CharacterPlayAnimationClip : CharacterActionBase
    {
        protected override async Task Proccess(CharacterAnimator character, IPauseHandler ph, CancellationToken token)
        {
            await character.PlayAnimationAsync(_animationClipName, ph, token);
        }

        [SerializeField]
        private string _animationClipName;
    }
}