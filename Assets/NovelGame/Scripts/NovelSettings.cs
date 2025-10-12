using UnityEngine;

namespace NovelGame.Scripts
{
    /// <summary>
    ///     ノベルゲームのセッティングデータ
    /// </summary>
    [CreateAssetMenu(fileName = nameof(NovelSettings), menuName = nameof(NovelSettings), order = 0)]
    public class NovelSettings : ScriptableObject
    {
        public float ShowSpeed => _showSpeed;

        [SerializeField, Tooltip("一秒間に表示される文字数")]
        private float _showSpeed = 10;
    }
}