using UnityEngine;

namespace GridDungeon.Scripts
{
    [CreateAssetMenu(fileName = "WorldConfig", menuName = "GridDungeon/WorldConfig", order = 1)]
    public class WorldConfig : ScriptableObject
    {
        public float GridScale => _gridScale;
        public Vector2 GridOffset => _gridOffset;

        [SerializeField]
        private Vector2 _gridOffset;
        [SerializeField, Min(0.1f)]
        private float _gridScale = 1;
    }
}