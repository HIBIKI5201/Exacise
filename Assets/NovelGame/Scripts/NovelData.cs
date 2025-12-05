using System;
using UnityEngine;

namespace NovelGame.Scripts
{
    /// <summary>
    ///     ノベルゲームのデータを保持するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = nameof(NovelData), menuName = nameof(NovelData), order = 0)]
    public class NovelData : ScriptableObject
    {
        public TextData this[int index] => _textDatas[index];
        public int Length => _textDatas.Length;
        public TextData[] TextDatas => _textDatas;

        [SerializeField]
        private TextData[] _textDatas =
        {
            new TextData("こんにちは、世界！"),
            new TextData("これはノベルゲームのサンプルテキストです。"),
            new TextData("Unityでノベルゲームを作成しましょう！")
        };

        [Serializable]
        public class TextData
        {
            public TextData(string text)
            {
                _text = text;
            }

            public string Name => _name;
            public string Text => _text;
            public string Action => _action;
            public bool IsWaitForInput => _isWaitForInput;
            public IAction[] ActionObject => _actionObject;

            [SerializeField]
            private string _name = string.Empty;
            [SerializeField, TextArea(3, 10)]
            private string _text = string.Empty;
            [SerializeField]
            private string _action = string.Empty;
            [SerializeReference, SubclassSelector]
            private IAction[] _actionObject = null;
            [SerializeField]
            private bool _isWaitForInput = true;
        }
    }
}