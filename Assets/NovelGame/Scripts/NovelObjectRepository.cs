namespace NovelGame.Scripts
{
    /// <summary>
    ///     ノベルゲームのオブジェクトリポジトリを管理します。
    /// </summary>
    public class NovelObjectRepository
    {
        public NovelObjectRepository(
            BackGroundUIManager backGroundUIManager,
            CharacterAnimator[] characterAnimators,
            MassageWindowPresenter massageWindowPresenter
            )
        {
            _backGroundUIManager = backGroundUIManager;
            _characterAnimators = characterAnimators;
            _massageWindowPresenter = massageWindowPresenter;
        }

        public BackGroundUIManager BackGroundUIManager => _backGroundUIManager;
        public CharacterAnimator[] CharacterAnimators => _characterAnimators;
        public MassageWindowPresenter MassageWindowPresenter => _massageWindowPresenter;

        private readonly BackGroundUIManager _backGroundUIManager;
        private readonly CharacterAnimator[] _characterAnimators;
        private MassageWindowPresenter _massageWindowPresenter;
    }
}
