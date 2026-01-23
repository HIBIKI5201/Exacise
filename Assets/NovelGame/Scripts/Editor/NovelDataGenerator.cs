using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

namespace NovelGame.Scripts
{
    public class NovelDataGenerator : EditorWindow
    {
        [MenuItem("Tools/" + nameof(NovelDataGenerator))]
        public static void Window()
        {
            NovelDataGenerator window = (NovelDataGenerator)GetWindow(typeof(NovelDataGenerator));
            window.Show();
        }

        private string _sheetName;

        private async void OnGUI()
        {
            _sheetName = EditorGUILayout.TextField(_sheetName);

            if (GUILayout.Button("生成"))
            {
                string csv = await GetCSV();
                NovelData data = NovelDataConverter.Execute(csv);
            }
        }

        private async ValueTask<string> GetCSV()
        {
            string url = NovelGameSettingsProvider.GetDataBaseURL();
            var builder = new StringBuilder(100);
            builder.Append("https://docs.google.com/spreadsheets/d/");
            builder.Append(url);
            builder.Append("/gviz/tq?tqx=out:csv&sheet=");
            builder.Append(_sheetName);

            UnityWebRequest request = UnityWebRequest.Get(builder.ToString());
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                return string.Empty;
            }

            return request.downloadHandler.text;
        }
    }
}