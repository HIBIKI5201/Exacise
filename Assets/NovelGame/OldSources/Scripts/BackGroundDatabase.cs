using System;
using UnityEngine;

namespace NovelGame.Scripts
{
    /// <summary>
    ///     ノベルゲームの背景データベースを管理します。
    /// </summary>
    [CreateAssetMenu(fileName = "BackGroundDatabase", menuName = "NovelGame/BackGroundDatabase", order = 1)]
    public class BackGroundDatabase : ScriptableObject
    {
        public Sprite this[string name] => Array.Find(_backgrounds, bg => bg.Name == name).Sprite;

        [SerializeField]
        private BackgroundData[] _backgrounds;

        [Serializable]
        private struct BackgroundData
        {
            public string Name;
            public Sprite Sprite;
        }
    }
}
