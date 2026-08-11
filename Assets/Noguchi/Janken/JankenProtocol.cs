using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
///  TODO
///  ・Jsonで通信
///  ・暗号化
///  ・PlayerPrefsで結果保存
/// </summary>

public class JankenProtocol : MonoBehaviour
{
    [SerializeField]
    private string _url;
    [SerializeField]
    private JankenView _view;

    [SerializeField]
    private float _delay = 0.5f;

    private Task _task;

    public void Start()
    {
        _view.BattleStarted += Call;
    }

    public void OnDestroy()
    {
        _view.BattleStarted -= Call;
        _view.Dispose();
    }

    private void Call(JankenHandEnum hand)
    {
        if (_task?.Status == TaskStatus.Running) { return; }
        _task = Run(hand, destroyCancellationToken).AsTask();
    }

    private async ValueTask Run(JankenHandEnum hand, CancellationToken token)
    {
        JankenRequester requester = new(_url);
        ValueTask<JankenRequester.Result> task = requester.CallAPI(hand);

        try
        {
            await _view.BattleStartPerformance(token);
            await task;

            JankenRequester.Result result = task.Result;
            Debug.Log(result);
            _view.SetResult(result);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return;
        }
    }
}
