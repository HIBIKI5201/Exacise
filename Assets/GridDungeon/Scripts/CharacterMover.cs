using UnityEngine;

namespace GridDungeon.Scripts
{
    /// <summary>
    /// キャラクターを移動させるためのクラス
    /// </summary>
    public class CharacterMover : MonoBehaviour
    {
        [SerializeField]
        private WorldConfig _worldConfig;

        private void Awake()
        {
            Debug.Assert(_worldConfig != null, "WorldConfig is not assigned.");
        }

        private void Start()
        {
            if (_worldConfig == null) return;

            // グリッドにスナップさせる
            Vector3 pos = transform.position;

            pos /= _worldConfig.GridScale;

            pos = Vector3Int.FloorToInt(pos);
            pos += new Vector3(_worldConfig.GridOffset.x, 0, _worldConfig.GridOffset.y);

            transform.position = pos;
        }
    }
}