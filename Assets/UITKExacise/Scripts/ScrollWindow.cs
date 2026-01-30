using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UITKExacise.Scripts
{
    [UxmlElement]
    public partial class ScrollWindow : VisualElement
    {
        public ScrollWindow()
        {
            VisualTreeAsset vta = Resources.Load<VisualTreeAsset>("UITK/uxml/ScrollWindow");
            vta.CloneTree(this);
            VisualElement window = this.Q<VisualElement>("window");
            Button button = this.Q<Button>("button");
            button.clicked += OnClick;
        }

        public event Action OnClieckedButton;

        private void OnClick()
        {
            Debug.Log("スクロールウィンドウのボタンが押された");
            OnClieckedButton?.Invoke();
        }
    }
}
