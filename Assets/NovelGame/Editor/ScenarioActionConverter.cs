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
            ScenarioArguments args = new(match.Groups.Count > 2 && !string.IsNullOrEmpty(match.Groups[2].Value)
                ? match.Groups[2].Value.Split(", ").Select(s => s.Trim()).ToArray()
                : Array.Empty<string>());

            try
            {
                switch (command)
                {
                    case nameof(ActorAnime):
                        return new ActorAnime(args[0], args[1]);

                    case nameof(ActorEnter):
                        return new ActorEnter(args[0], args[1], new(args[2], args[3]));

                    case nameof(ActorExit):

                        return new ActorExit(args[0], args[1]);

                    case nameof(ActorMove):
                        return new ActorMove(args[0], new(args[1], args[2]), args[3]);

                    case nameof(BackGroundCrossFade):
                        return new BackGroundCrossFade(args[0], args[1]);

                    case nameof(BackGroundChange):
                        return new BackGroundChange(args[0]);

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
