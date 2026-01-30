using UnityEngine;
using UnityEngine.UIElements;

namespace UITKExacise.Scripts
{
    [RequireComponent(typeof(UIDocument))]
    public class DocumentLocater : MonoBehaviour
    {
        [SerializeField]
        private string _buttonName;

        private UIDocument _document;
        private VisualElement _root;

        private Button _button;

        private void Awake()
        {
            if (!TryGetComponent(out UIDocument document)) { return; }
            
            _document = document;
            _root = document.rootVisualElement;

            ScrollWindow sw = _root.Q<ScrollWindow>();
            sw.OnClieckedButton += Clicked;
        }

        private void Clicked() => Debug.Log("何か押された");
    }
}
