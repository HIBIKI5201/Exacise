using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelGame.Master.Scripts.UI
{
    [Serializable]
    public class ChoiceButtonPresenter
    {
        public event Action<int> OnClicked;

        public VisualElement Root => _root;

        public VisualElement Create()
        {
            VisualElement root = new VisualElement();
            InitRoot(root);

            _root = root;
            return root;
        }

        public void PopButton(string text, int lineIndex, Vector2Int pos)
        {
            VisualElement e = CreateButton(text, () => ButtonClicked(lineIndex));
            MoveButton(e, pos);
        }

        [SerializeField]
        private VisualTreeAsset _choiceButtonUXML;

        private VisualElement _root;

        private List<VisualElement> _buttons = new();
        private int _activeButtonsCount;

        private void InitRoot(VisualElement root)
        {
            root.pickingMode = PickingMode.Ignore;

            IStyle style = root.style;
            style.position = Position.Absolute;
            style.width = Length.Percent(100);
            style.height = Length.Percent(100);

            style.borderBottomColor = Color.red;
            style.borderLeftColor = Color.red;
            style.borderRightColor = Color.red;
            style.borderTopColor = Color.red;
            style.borderBottomWidth = 5;
            style.borderLeftWidth = 5;
            style.borderRightWidth = 5;
            style.borderTopWidth = 5;
        }

        private VisualElement CreateButton(string text, Action action)
        {
            VisualElement element = null;
            if (_activeButtonsCount < _buttons.Count)
            {
                element = _buttons[_activeButtonsCount];
                element.style.display = DisplayStyle.Flex;
            }
            else
            {
                element = _choiceButtonUXML.Instantiate();
                element.style.position = Position.Absolute;
                _buttons.Add(element);
                _root.Add(element);
            }
            _activeButtonsCount++;

            Button button = element.Q<Button>();
            button.clicked += action;
            button.text = text;

            return element;
        }

        private void ButtonClicked(int index)
        {
            OnClicked?.Invoke(index);
            for (int i = 0; i < _activeButtonsCount; i++)
            {
                VisualElement e = _buttons[i];
                e.style.display = DisplayStyle.None;
            }

            _activeButtonsCount = 0;
        }

        private void MoveButton(VisualElement element, Vector2Int pos)
        {
            Vector2Int center = new(Screen.width / 2, Screen.height / 2);

            int x = pos.x + center.x;
            int y = Screen.height - pos.y - center.y;

            element.style.left = new(new Length(x, LengthUnit.Pixel));
            element.style.top = new(new Length(y, LengthUnit.Pixel));
        }
    }
}
