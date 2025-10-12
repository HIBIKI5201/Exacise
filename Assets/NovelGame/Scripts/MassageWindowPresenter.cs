using System;
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

        public void SetName(string name)
        {
            _nameLabel.text = name;
        }

        public void SetMassage(string massage)
        {
            _messageLabel.text = massage;
        }

        [SerializeField]
        private string _nameLabelName = "name";
        [SerializeField]
        private string _messageLabelName = "message";
        [SerializeField]
        private string _clickButtonName = "click-button";

        private UIDocument _document;

        private Label _nameLabel;
        private Label _messageLabel;
        private Button _clickButton;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
        }

        private void Start()
        {
            var root = _document.rootVisualElement;
            _nameLabel = root.Q<Label>(_nameLabelName);
            _messageLabel = root.Q<Label>(_messageLabelName);
            _clickButton = root.Q<Button>(_clickButtonName);

            Debug.Assert(_nameLabel != null, $"{_nameLabelName}という名前のLabelが見つかりません。", this);
            Debug.Assert(_messageLabel != null, $"{_messageLabelName}という名前のLabelが見つかりません。", this);
            Debug.Assert(_clickButton != null, $"{_clickButtonName}という名前のButtonが見つかりません。", this);
        }
    }
}