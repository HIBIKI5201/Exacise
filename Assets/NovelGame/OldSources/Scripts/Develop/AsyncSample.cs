using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncSample : MonoBehaviour
{
    public async void Start()
    {
        Debug.Log("処理開始");
        await RunTask(destroyCancellationToken);
        Debug.Log("すべての処理が完了しました");
    }

    // 以下の ViewAsync と LoadAsync メソッドを並列で実行し、
    // 両方の処理が完了したら「すべての処理が完了しました」とログに出力するコードを記述してください。

    /// <summary>
    ///     両方のタスクを並列で実行するメソッド
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private async ValueTask RunTask(CancellationToken token = default)
    {
        ValueTask task = StartCoroutineAsync(ViewAsync(), token);
        Task loadTask = LoadAsync();

        await Task.WhenAll(task.AsTask(), loadTask);
    }

    /// <summary>
    ///     コルーチンをラップして非同期で実行するメソッド
    /// </summary>
    /// <param name="enumerator"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async ValueTask StartCoroutineAsync(IEnumerator enumerator, CancellationToken token = default)
    {
        TaskCompletionSource<bool> source =new();
        Coroutine coroutine = StartCoroutine(Wrapper(enumerator, source));
        using CancellationTokenRegistration registration = token.Register(() =>
        {
            StopCoroutine(coroutine);
            source.TrySetCanceled();
        });

        try
        {
            await source.Task;
        }
        catch (OperationCanceledException)
        {
            source.TrySetCanceled();
            Debug.Log("Coroutine was cancelled.");
        }

        IEnumerator Wrapper(IEnumerator enumerator, TaskCompletionSource<bool> source)
        {
            yield return enumerator;
            source.TrySetResult(true);
        }
    }

    //以下は別のライブラリでコードは編集してはならない

    // データ表示演出を非同期で実行するメソッドを想定
    private IEnumerator ViewAsync()
    {
        var duration = UnityEngine.Random.Range(3, 5f);
        var elapsed = 0f;
        var startTime = Time.time;
        while (elapsed < duration)
        {
            elapsed = Time.time - startTime;
            Debug.Log($"データ表示演出待ち... {elapsed}");
            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log($"データ表示演出完了");
    }

    // データ読み込みを非同期で実行するメソッドを想定
    private async Task LoadAsync()
    {
        var duration = UnityEngine.Random.Range(3, 5f);
        Debug.Log($"データ読み込み開始... {duration}秒待機");
        await Task.Delay(TimeSpan.FromSeconds(duration));
        Debug.Log("データ読み込み完了");

    }
}