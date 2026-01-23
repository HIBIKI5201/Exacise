using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelGame.Scripts
{
    /// <summary>
    ///     ノベルゲームのウィンドウを管理します。
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    [DefaultExecutionOrder(-100)]
    public class MassageWindowPresenter : MonoBehaviour
    {
        public event Action OnClickButton
        {
            add => _clickButton.clicked += value;
            remove => _clickButton.clicked -= value;
        }

        public Color MessageLabelColor => _messageLabel.style.color.value;
        public SkipDialogManager SkipDialogManager => _skipDialogManager;

        public void SetName(string name)
        {
            _nameLabel.text = name;
        }

        public void SetMassage(string massage)
        {
            _messageLabel.text = massage;
        }

        public async Task FadeInBoard(float duration, IPauseHandler ph, CancellationToken token = default)
        {
            _fadeBoardElement.style.opacity = 0f;
            float elapsed = 0f;

            try
            {
                while (elapsed < duration)
                {
                    float alpha = Mathf.Clamp01(elapsed / duration);
                    _fadeBoardElement.style.opacity = alpha;
                    elapsed += Time.deltaTime;

                    await Awaitable.NextFrameAsync(token);
                    await ph.WaitResumeAsync(token);
                }
            }
            finally
            {
                _fadeBoardElement.style.opacity = 1f;
            }
        }

        public async Task FadeOutBoard(float duration, IPauseHandler ph, CancellationToken token = default)
        {
            _fadeBoardElement.style.opacity = 1f;
            float elapsed = 0f;

            try
            {
                while (elapsed < duration)
                {
                    float alpha = 1f - Mathf.Clamp01(elapsed / duration);
                    _fadeBoardElement.style.opacity = alpha;
                    elapsed += Time.deltaTime;

                    await Awaitable.NextFrameAsync(token);
                    await ph.WaitResumeAsync(token);

                }
            }
            finally
            {
                _fadeBoardElement.style.opacity = 0f;
            }
        }

        [SerializeField]
        private string _nameLabelName = "name";
        [SerializeField]
        private string _messageLabelName = "message";
        [SerializeField]
        private string _clickButtonName = "click-button";
        [SerializeField]
        private string _skipButtonName = "skip-button";
        [SerializeField]
        private string _skipRootName = "skip-root";
        [SerializeField]
        private string _skipDialogName = "skip-dialog";

        [SerializeField]
        private string _fadeBoard = "fade-board";

        private UIDocument _document;

        private Label _nameLabel;
        private Label _messageLabel;
        private Button _clickButton;
        private Button _skipButton;
        private VisualElement _skipRoot;
        private VisualElement _skipDialog;

        private VisualElement _fadeBoardElement;

        private SkipDialogManager _skipDialogManager;

        private async Task Awake()
        {
            _document = GetComponent<UIDocument>();

            CancellationTokenSource s = new();
        }

        private void Start()
        {
            var root = _document.rootVisualElement;
            _nameLabel = root.Q<Label>(_nameLabelName);
            _messageLabel = root.Q<Label>(_messageLabelName);
            _clickButton = root.Q<Button>(_clickButtonName);
            _skipButton = root.Q<Button>(_skipButtonName);
            _skipRoot = root.Q<VisualElement>(_skipRootName);
            _skipDialog = root.Q<VisualElement>(_skipDialogName);
            _fadeBoardElement = root.Q<VisualElement>(_fadeBoard);

            Debug.Assert(_nameLabel != null, $"{_nameLabelName}という名前のLabelが見つかりません。", this);
            Debug.Assert(_messageLabel != null, $"{_messageLabelName}という名前のLabelが見つかりません。", this);
            Debug.Assert(_clickButton != null, $"{_clickButtonName}という名前のButtonが見つかりません。", this);
            Debug.Assert(_skipButton != null, $"{_skipButtonName}という名前のButtonが見つかりません。", this);
            Debug.Assert(_skipDialog != null, $"{_skipDialogName}という名前のVisualElementが見つかりません。", this);
            Debug.Assert(_fadeBoardElement != null, $"{_fadeBoard}という名前のVisualElementが見つかりません。", this);

            if (_skipButton != null && _skipDialog != null) { _skipDialogManager = new(_skipButton, _skipRoot, _skipDialog); }
            else { Debug.LogError("スキップ機能が生成されませんでした"); }
        }
    }
}