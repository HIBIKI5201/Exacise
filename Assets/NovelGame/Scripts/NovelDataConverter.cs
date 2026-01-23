using NovelGame.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static NovelGame.Scripts.NovelData;

namespace NovelGame.Scripts
{
    public static class NovelDataConverter
    {
        public static NovelData Execute(string text)
        {
            StringReader reader = new StringReader(text);
            List<TextData> textDatas = new();

            while(reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                string[] elements = line.Split(", ");

                Span<string> actions = new(elements, 4, elements.Length - 3);
                IAction[] actiondata = new IAction[actions.Length];
                for (int i = 0; i < actions.Length; i++)
                {
                    actiondata[i] = ActionConverter.ActionConvert(actions[i]);
                }

                
            }
        }
    }
}
