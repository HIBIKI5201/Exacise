using UnityEngine;

namespace NovelGame.Scripts
{
    /// <summary>
    ///     ノベルゲームのオブジェクトリポジトリを管理します。
    /// </summary>
    public class NovelObjectRepository
    {
        public NovelObjectRepository(
            BackGroundUIManager backGroundUIManager,
            CharacterAnimator[] characterAnimators
            )
        {
            _backGroundUIManager = backGroundUIManager;
            _characterAnimators = characterAnimators;
        }

        public BackGroundUIManager BackGroundUIManager => _backGroundUIManager;
        public CharacterAnimator[] CharacterAnimators => _characterAnimators;

        private readonly BackGroundUIManager _backGroundUIManager;
        private readonly CharacterAnimator[] _characterAnimators;
    }
}
