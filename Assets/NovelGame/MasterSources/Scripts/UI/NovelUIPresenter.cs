using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelGame.Master.Scripts.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class NovelUIPresenter : MonoBehaviour
    {
        [SerializeField]
        private string _messageWindowName;

        [SerializeField]
        private MessageWindowViewModel _vm;

        private UIDocument _document;
        private VisualElement _root;

        private VisualElement _messageWindow;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _root = _document.rootVisualElement;
        }

        private void Start()
        {
            _vm = ScriptableObject.CreateInstance<MessageWindowViewModel>();
            _messageWindow = _root.Q(_messageWindowName);
            _messageWindow.dataSource = _vm;
        }
    }
}
