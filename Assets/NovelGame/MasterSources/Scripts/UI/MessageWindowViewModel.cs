using UnityEngine;

namespace NovelGame.Master.Scripts.UI
{
    public class MessageWindowViewModel : ScriptableObject
    {
        public void SetText(string characterName, string messageText)
        {
            _characterName = characterName;
            _messageText = messageText;
        }

        [SerializeField] private string _characterName = string.Empty;
        [SerializeField] private string _messageText = string.Empty;
    }
}