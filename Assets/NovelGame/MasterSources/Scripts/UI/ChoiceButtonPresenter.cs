using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NovelGame.Master.Scripts.UI
{
    [Serializable]
    public class ChoiceButtonPresenter
    {
        public VisualElement Create()
        {
            VisualElement root = new VisualElement();
            InitRoot(root);

            return root;
        }

        [SerializeField]
        private VisualTreeAsset _choiceButtonUXML;

        private void InitRoot(VisualElement root)
        {
            root.pickingMode = PickingMode.Ignore;

            IStyle style = root.style;
            style.backgroundColor = Color.red;
            style.width = new Length(100, LengthUnit.Percent);
            style.height = new Length(100, LengthUnit.Percent);
        }
    }
}
