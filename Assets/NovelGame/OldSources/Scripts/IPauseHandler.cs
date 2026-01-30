using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IPauseHandler
{
    public bool IsPaused { get; }
    public async ValueTask WaitResumeAsync(CancellationToken token = default)
    {
        if (!IsPaused) { return; }

        while (IsPaused) { await Awaitable.NextFrameAsync(token); }
    }
}

