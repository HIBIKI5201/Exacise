using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.UseCase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NovelGame.Master.Scripts.Editor
{
    public static class ScenarioDataConverter
    {
        public static ScenarioDataAsset Execute(string text, ref StringBuilder log)
        {
            var textDatas = new List<ScenarioNode>();
            StringReader reader = new StringReader(text);

            int lineNumber = 0;
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) { continue; }

                string[] elements = SplitCsvLine(line);

                if (elements.Length < 3)
                {
                    log.AppendLine($"要素が足りません\n".ErrorString());
                    return null;
                }

                if (!bool.TryParse(elements[2], out bool isWaitForInput))
                {
                    isWaitForInput = true; // 失敗時にデフォルト値にする。
                    log.AppendLine($"line{lineNumber.OneBased()}: bool値のパースに失敗したためデフォルト値(true)を使用します\n".WarningString());
                }

                Span<string> actions = new(elements, 3, elements.Length - 3);
                List<IScenarioAction> actiondata = new(actions.Length);
                for (int i = 0; i < actions.Length; i++)
                {
                    if (string.IsNullOrEmpty(actions[i])) { continue; }

                    try
                    {
                        IScenarioAction action = ScenarioActionConverter.ActionConvert(actions[i], ref log);
                        if (action != null) { actiondata.Add(action); }
                    }
                    catch (Exception e)
                    {
                        log.AppendLine($"line{lineNumber.OneBased()}, index {i}: action is could not convert\n{e.Message}\n");
                    }
                }

                ScenarioNode textData = new(elements[1], elements[0], isWaitForInput, actiondata.ToArray());
                textDatas.Add(textData);

                lineNumber++;
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

        private static int OneBased(this int n) => n + 1;
    }
}
