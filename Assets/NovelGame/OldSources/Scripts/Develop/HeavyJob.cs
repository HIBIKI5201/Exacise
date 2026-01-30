using Unity.Burst;
using UnityEngine;

public static class HeavyJob
{
    public static bool IsPrime(int n)
    {
        if (n < 2) { return false; }
        if (n % 2 == 0 && n != 2) { return false; }

        int limit = (int)Mathf.Sqrt(n);
        for (int i = 3; i <= limit; i += 2)
        {
            if (n % i == 0) { return false; }
        }

        return true;
    }

    [BurstCompile]
    public static bool IsPrimeBurst(int n)
    {
        if (n < 2) { return false; }
        if (n % 2 == 0 && n != 2) { return false; }

        int limit = (int)Mathf.Sqrt(n);
        for (int i = 3; i <= limit; i += 2)
        {
            if (n % i == 0) { return false; }
        }

        return true;
    }

}
