using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.UseCase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NovelGame.Master.Scripts.Editor
{
    public class ScenarioDataConverter
    {
        public static ScenarioDataAsset Execute(string text)
        {
            var textDatas = new List<ScenarioNode>();
            StringReader reader = new StringReader(text);

            int lineNumber = 0;
            while (reader.Peek() != -1)
            {
                lineNumber++;
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) { continue; }

                string[] elements = SplitCsvLine(line);

                if (elements.Length < 3)
                {
                    Debug.LogWarning($"不正なフォーマットの行をスキップしました: Line {lineNumber} - {line}");
                    continue;
                }

                Span<string> actions = new(elements, 3, elements.Length - 3);
                List<IScenarioAction> actiondata = new(actions.Length);
                for (int i = 0; i < actions.Length; i++)
                {
                    if (string.IsNullOrEmpty(actions[i])) { continue; }

                    IScenarioAction action = ScenarioActionConverter.ActionConvert(actions[i]);
                    if (action != null) { actiondata.Add(action); }
                }

                if (!bool.TryParse(elements[2], out bool isWaitForInput))
                {
                    isWaitForInput = true; // 失敗時にデフォルト値にする。
                    Debug.LogWarning($"bool値のパースに失敗したためデフォルト値(true)を使用します: Line {lineNumber}");
                }

                ScenarioNode textData = new(elements[1], elements[0], isWaitForInput, actiondata.ToArray());
                textDatas.Add(textData);
            }

            ScenarioDataAsset data = ScriptableObject.CreateInstance<ScenarioDataAsset>();
            data.SetScenarioNodes(textDatas.ToArray());
            return data;
        }

        private static string[] SplitCsvLine(string line)
        {
            // カンマの後に偶数個の引用符が続く場所でのみカンマで分割する正規表現
            string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
            string[] values = Regex.Split(line, pattern);

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim().Trim('"');
            }

            return values;
        }
    }
}
