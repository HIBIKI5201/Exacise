using UnityEngine;

namespace UITKExacise.Scripts
{
    [CreateAssetMenu(fileName = nameof(CharacterData), menuName = nameof(CharacterData))]
    public class CharacterData : ScriptableObject
    {
        public string CharacterName => _name;
        public Texture2D Icon => _icon;

        [SerializeField]
        private string _name;
        [SerializeField]
        private Texture2D _icon;
    }
}
