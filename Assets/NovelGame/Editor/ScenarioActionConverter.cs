using NovelGame.Master.Scripts.UseCase;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NovelGame.Master.Scripts.Editor
{
    public static class ScenarioActionConverter
    {
        public static IScenarioAction ActionConvert(string actionInfo, ref StringBuilder log)
        {
            // コマンド名と角括弧で囲まれた引数リストを抽出する正規表現
            // 例: Command[arg1, arg2], Command[arg1], Command[], Command
            var pattern = @"^(\w+)(?:\[([^\]]*)\])?$";
            var match = Regex.Match(actionInfo, pattern);

            if (!match.Success)
            {
                throw new Exception($"書式が不正です: {actionInfo}".ErrorString());
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
                    case nameof(ActorAnime): return new ActorAnime(args[0], args[1]);
                    case nameof(ActorEnter): return new ActorEnter(args[0], args[1], new(args[2], args[3]));
                    case nameof(ActorExit): return new ActorExit(args[0], args[1]);
                    case nameof(ActorMove): return new ActorMove(args[0], args[1], new(args[2], args[3]));
                    case nameof(BackGroundCrossFade): return new BackGroundCrossFade(args[0], args[1]);
                    case nameof(BackGroundChange): return new BackGroundChange(args[0]);
                    case nameof(DisableClick): return new DisableClick();
                    case nameof(ShowButton): return new ShowButton(args[0], args[1] - 1, new(args[2], args[3])); // jumpLineを1オリジンから0オリジンに変換。
                    default:
                        throw new FormatException();
                }
            }
            catch (IndexOutOfRangeException e)
            {
                throw new Exception($"コマンド '{command}' に対する引数が足りていません".ErrorString());
            }
            catch (FormatException e)
            {
                throw new Exception($"不明なコマンドです: {command}".WarningString());

            }
            catch (Exception e)
            {
                throw new Exception($"コマンド '{command}' の引数処理中にエラーが発生しました。Args: [{args.ToString()}]\n{e}".ErrorString());
            }
        }
    }
}
