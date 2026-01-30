using System.Collections;
using UnityEngine;

public class MultiThreadCoroutine : MonoBehaviour
{
    [SerializeField] private float _waitTime = 5f;

    [ContextMenu("Run MultiThreadCoroutine")]
    public void Run()
    {
        StartCoroutine(Enumerator());
    }

    private IEnumerator Enumerator()
    {
        // 両方のコルーチンを個別に開始
        IEnumerator task1 = AsyncTask();
        IEnumerator task2 = WaitPressKey();

        // どちらかが終わるまで待つ
        yield return new WaitUntil(() => !task1.MoveNext() || !task2.MoveNext());

        Debug.Log("いずれかのタスクが完了しました。");
    }

    private IEnumerator AsyncTask()
    {
        Debug.Log("Task Start");
        float elapsedTime = 0f;
        while (elapsedTime < _waitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Task End");
    }

    private IEnumerator WaitPressKey()
    {
        Debug.Log("Press any key to continue...");
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        Debug.Log("Key pressed!");
    }
}
