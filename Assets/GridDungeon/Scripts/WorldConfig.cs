using UnityEngine;

namespace GridDungeon.Scripts
{
    /// <summary>
    /// ワールド全体の設定を保持するScriptableObject。
    /// グリッドのスケールやオフセットなどを定義します。
    /// </summary>
    [CreateAssetMenu(fileName = "WorldConfig", menuName = "GridDungeon/WorldConfig", order = 1)]
    public class WorldConfig : ScriptableObject
    {
        /// <summary>
        /// グリッドのスケール。
        /// </summary>
        public float GridScale => _gridScale;

        /// <summary>
        /// グリッドのオフセット。
        /// </summary>
        public Vector2 GridOffset => _gridOffset;

        [SerializeField, Tooltip("グリッドのオフセット")]
        private Vector2 _gridOffset;

        [SerializeField, Min(0.1f), Tooltip("グリッドのスケール")]
        private float _gridScale = 1;
    }
}
