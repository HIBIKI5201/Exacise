using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NovelGame.Master.Scripts.Editor
{
    public class NovelGameSettingsProvider : SettingsProvider
    {
        public NovelGameSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes, keywords)
        {
            _databaseURL = GetDataBaseURL();
        }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            // SettingsScopeをProjectにします
            return new NovelGameSettingsProvider(SETTING_PATH, SettingsScope.Project, null);
        }

        public static string GetDataBaseURL() => EditorPrefs.GetString(DATABASE_URL_KEY);

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.LabelField("データベースのURL");
            _databaseURL = EditorGUILayout.TextField(_databaseURL);

            if (GUILayout.Button("保存"))
            {
                EditorPrefs.SetString(DATABASE_URL_KEY, _databaseURL);
                Debug.Log($"保存しました。\ndatabase URL: {_databaseURL}");
            }
        }

        private const string SETTING_PATH = "Project/PJ/NovelGame";

        private const string DATABASE_URL_KEY = "DataBase_URL_Key";

        private string _databaseURL;
    }
}
