using MetaHorror.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace MetaHorror.Scripts
{
    public class DesktopViewer : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        private void Start()
        {
#if UNITY_STANDALONE_WIN
            Texture2D tex = WindowsMetaProcessor.Capture();
            if (tex != null)
            {
                _image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            }
#endif
        }
    }
}



