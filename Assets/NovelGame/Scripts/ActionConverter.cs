using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

namespace NovelGame.Scripts
{
    public static class ActionConverter
    {
        public static IAction ActionConvert(string actionInfo)
        {
            var pattern = @"^(\w+)\[([^,]+),([^\]]+)\]$";
            var match = Regex.Match(actionInfo, pattern);

            if (!match.Success)
            {
                Debug.LogError("フォーマット不正");
                return null;
            }

            string command = match.Groups[1].Value;

            string[] args = System.Array.Empty<string>();

            if (match.Groups[2].Success && !string.IsNullOrEmpty(match.Groups[2].Value))
            {
                args = match.Groups[2].Value
                    .Split(',')
                    .Select(x => x.Trim())
                    .ToArray();
            }

            switch (command)
            {
                case "CharacterFadeIn": return new CharacterFadeIn(int.Parse(args[1]), args[0]);
            }

            return null;
        }
    }
}
