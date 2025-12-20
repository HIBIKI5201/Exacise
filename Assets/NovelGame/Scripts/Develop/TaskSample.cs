using System;
using UnityEngine;

public class TaskSample : MonoBehaviour
{
    [SerializeField, Min(1)]
    private float _rotateSpeed = 1;

    [SerializeField, Min(1)]
    private int _jobSize = 1;

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.Rotate(0, _rotateSpeed, 0);
    }

    private static (int trueCount, int falseCount) CountBoolen(ReadOnlySpan<bool> bools)
    {
        int trueCount = 0;
        int falseCount = 0;

        for (int i = 0; i < bools.Length; i++)
        {
            if (bools[i]) { trueCount++; }
            else { falseCount++; }
        }
        return (trueCount, falseCount);
    }
}
