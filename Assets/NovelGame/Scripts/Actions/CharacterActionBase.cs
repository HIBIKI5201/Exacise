using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Scripts
{
    /// <summary>
    ///     キャラクターアクションの基底クラスを表します。
    /// </summary>
    public abstract class CharacterActionBase : IAction
    {
        public async Task ExcuteAsync(NovelObjectRepository repository, CancellationToken token = default)
        {
            CharacterAnimator character =
                Array.Find(repository.CharacterAnimators, c => c.Name == _characterName);
            await Proccess(character, token);
        }

        [SerializeField]
        protected string _characterName;

        protected abstract Task Proccess(CharacterAnimator character, CancellationToken token);
    }
}
