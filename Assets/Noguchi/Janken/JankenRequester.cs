using System;
using System.Net.Http;
using System.Threading.Tasks;
using Unity.Serialization;
using UnityEngine;
using UnityEngine.Networking;

public class JankenRequester
{
    public JankenRequester(string url)
    {
        _url = url;
    }

    private JankenRequester() { }

    public async ValueTask<Result> CallAPI(JankenHandEnum janken)
    {
        string get = $"?hand={(int)janken}";
        using UnityWebRequest request = UnityWebRequest.Get(_url + get);
        await request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            throw new HttpRequestException(request.error);
        }

        string[] array = request.downloadHandler.text.Split(',');
        if (array.Length < 3) { throw new ParseErrorException("長さが足りません"); }
        (int self, int opponent, int r) = Parse(new ReadOnlySpan<string>(array, 0, 3));
        Result result = new Result((JankenHandEnum)self, (JankenHandEnum)opponent, (JankenResultEnum)r);
        return result;
    }

    (int self, int opponent, int r) Parse(ReadOnlySpan<string> input)
    {
        bool rr = int.TryParse(input[0], out int r);
        bool ar = int.TryParse(input[1], out int a);
        bool br = int.TryParse(input[2], out int b);

        if (!ar || !br || !rr) { throw new ParseErrorException("数値に変換できませんでした"); }

        return (a + 1, b + 1, r);
    }

    [Serializable]
    public readonly struct Result
    {
        internal Result(JankenHandEnum self, JankenHandEnum opponent,
            JankenResultEnum result)
        {
            _self = self;
            _opponent = opponent;
            _result = result;
        }

        public JankenHandEnum Self => _self;
        public JankenHandEnum Opponent => _opponent;
        public JankenResultEnum ResultType => _result;

        public override string ToString() => $"{_self},{_opponent},{_result}";

        private readonly JankenHandEnum _self;
        private readonly JankenHandEnum _opponent;
        private readonly JankenResultEnum _result;
    }

    private readonly string _url;
}
