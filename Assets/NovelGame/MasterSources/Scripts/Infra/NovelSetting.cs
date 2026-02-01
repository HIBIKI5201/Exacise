using UnityEngine;
using NovelGame.Master.Scripts.Utility;

namespace NovelGame.Master.Scripts.Infra
{
    [CreateAssetMenu(fileName = nameof(NovelSetting),
        menuName = InfraContraint.ASSET_PATH + nameof(NovelSetting))]
    public class NovelSetting : ScriptableObject
    {
        public float TextSpeed => _textSpeed;

        [SerializeField, Min(1)]
        private float _textSpeed = 30f;
    }
}
