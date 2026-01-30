using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TaskSample : MonoBehaviour
{
    BattleSystem<Buffer> _bs;

    private void Start()
    {
        _bs = new();

        _bs.Update(true, 100);
        _bs.Update(false, 100);

        _bs.Execute();
    }
}

public struct Buffer : IBuffer
{
    public int Total => _total;

    public void Add(BufferItem item)
    {
        _total += item.Value;
    }

    private int _total;
}

public interface IBuffer
{
    void Add(BufferItem item);
    int Total { get; }
}

public class BufferItem
{
    public int Value { get; }
    public BufferItem(int value)
    {
        Value = value;
    }
}

public class BattleSystem<T> where T : IBuffer, new()
{
    private T _buffer1 = new();
    private T _buffer2 = new();

    public void Update(bool flag, int value)
    {
        var buffer = flag ? _buffer1 : _buffer2;
        buffer.Add(new BufferItem(value));

        if (flag)
        {
            _buffer1.Add(new BufferItem(value));
        }
        else
        {
            _buffer2.Add(new BufferItem(value));
        }

        // その他の更新・集計処理
    }

    public void Execute()
    {
        Debug.Log(_buffer1.Total + _buffer2.Total);
    }
}