using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static NovelGame.Scripts.NovelData;

namespace NovelGame.Scripts
{
    public static class NovelDataConverter
    {
        public static NovelData Execute(string text)
        {
            var textDatas = new List<TextData>();
            StringReader reader = new StringReader(text);

            int lineNumber = 0;
            while (reader.Peek() != -1)
            {
                lineNumber++;
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)){ continue; }

                string[] elements = SplitCsvLine(line);

                if (elements.Length < 3)
                {
                    Debug.LogWarning($"不正なフォーマットの行をスキップしました: Line {lineNumber} - {line}");
                    continue;
                }
                
                Span<string> actions = new(elements, 3, elements.Length - 3);
                IAction[] actiondata = new IAction[actions.Length];
                for (int i = 0; i < actions.Length; i++)
                {
                    if (string.IsNullOrEmpty(actions[i])) { continue; }
                    actiondata[i] = ActionConverter.ActionConvert(actions[i]);
                }

                if (!bool.TryParse(elements[2], out bool isWaitForInput))
                {
                    isWaitForInput = true; // 失敗時にデフォルト値にする。
                    Debug.LogWarning($"bool値のパースに失敗したためデフォルト値(true)を使用します: Line {lineNumber}");
                }
                
                TextData textData = new(elements[1], elements[0], isWaitForInput, actiondata);
                textDatas.Add(textData);
            }

            NovelData data = ScriptableObject.CreateInstance<NovelData>();
            data.Initialize(textDatas.ToArray());
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
