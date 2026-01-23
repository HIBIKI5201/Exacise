using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelGame.Scripts
{
    public class SkipDialogManager : IPauseHandler
    {
        public SkipDialogManager(Button button, VisualElement root, VisualElement dialog)
        {
            _button = button;
            _root = root;
            _dialog = dialog;

            _skipButton = dialog.Q<Button>(SKIP_BUTTON_NAME);
            _cancelButton = dialog.Q<Button>(CANCEL_BUTTON_NAME);
            
            _button.clicked += HandleButtonCliecked;

            _cancelButton.clicked += Hide;
            OnClickedSkipButton += Hide;
            OnClickedSkipButton += () => Debug.Log("skipが押されました。");

            Hide();
        }

        public event Action OnClickedSkipButton
        {
            add => _skipButton.clicked += value;
            remove => _skipButton.clicked -= value;
        }

        public bool IsShowDialog => _isShowDialog;

        bool IPauseHandler.IsPaused => _isShowDialog;

        private const string SKIP_BUTTON_NAME = "skip";
        private const string CANCEL_BUTTON_NAME = "cancel";
        private readonly Button _button;
        private readonly VisualElement _root;
        private readonly VisualElement _dialog;

        private readonly Button _skipButton;
        private readonly Button _cancelButton;

        private bool _isShowDialog = false;

        private void HandleButtonCliecked()
        {
            if (!_isShowDialog) { Show(); }
            else { Hide(); }
        }

        private void Show()
        {
            _dialog.style.visibility = Visibility.Visible;
            _root.pickingMode = PickingMode.Position;
            _isShowDialog = true;
        }

        private void Hide()
        {
            _dialog.style.visibility = Visibility.Hidden;
            _root.pickingMode = PickingMode.Ignore;
            _isShowDialog = false;
        }
    }
}
