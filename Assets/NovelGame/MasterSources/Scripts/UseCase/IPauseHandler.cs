using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Master.Scripts.Utility
{
    /// <summary>
    ///     一時停止の状態を扱います。
    /// </summary>
    public interface IPauseHandler
    {
        public bool IsPaused { get; }

        public async ValueTask WaitResumeAsync(CancellationToken token = default)
        {
            if (!IsPaused) { return; }

            while (IsPaused) { await Awaitable.NextFrameAsync(token); }
        }
    }
}
