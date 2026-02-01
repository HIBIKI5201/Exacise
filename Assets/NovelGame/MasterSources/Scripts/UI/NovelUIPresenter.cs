using UnityEngine;
using UnityEngine.UIElements;

namespace NovelGame.Master.Scripts.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class NovelUIPresenter : MonoBehaviour
    {
        public void BindMessageWindowViewModel(MessageWindowViewModel vm)
        {
            _messageWindow = _root.Q(_messageWindowName);
            _messageWindow.dataSource = _vm;
        }

        [SerializeField]
        private string _messageWindowName;

        [SerializeField]
        private SkipWindowPresenter _skipWindowPresenter;

        [SerializeField]
        private MessageWindowViewModel _vm;

        private UIDocument _document;
        private VisualElement _root;

        private VisualElement _messageWindow;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _root = _document.rootVisualElement;
            _root.Add(_skipWindowPresenter.CreateView());
        }
    }
}
