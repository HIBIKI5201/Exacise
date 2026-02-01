using NovelGame.Master.Scripts.UseCase;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NovelGame.Master.Scripts.Editor
{
    public static class ScenarioActionConverter
    {
        public static IScenarioAction ActionConvert(string actionInfo)
        {
            // コマンド名と角括弧で囲まれた引数リストを抽出する正規表現
            // 例: Command[arg1, arg2], Command[arg1], Command[], Command
            var pattern = @"^(\w+)(?:\[([^\]]*)\])?$";
            var match = Regex.Match(actionInfo, pattern);

            if (!match.Success)
            {
                Debug.LogError($"書式が不正です: {actionInfo}");
                return null;
            }

            string command = match.Groups[1].Value;

            // 引数部分がマッチした場合のみ、カンマで分割する
            string[] args = match.Groups.Count > 2 && !string.IsNullOrEmpty(match.Groups[2].Value)
                ? match.Groups[2].Value.Split(", ").Select(s => s.Trim()).ToArray()
                : Array.Empty<string>();

            try
            {
                switch (command)
                {

                    default:
                        Debug.LogError($"不明なコマンドです: {command}");
                        return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"コマンド '{command}' の引数処理中にエラーが発生しました。Args: [{string.Join(", ", args)}]\n{e}");
                return null;
            }
        }
    }
}
