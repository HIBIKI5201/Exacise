using Codice.Client.BaseCommands;
using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.UseCase;
using NovelGame.Master.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Master.Scripts.Presenter
{
    public class ScenarioPlayer
    {
        public ScenarioPlayer(ScenarioDataAsset asset, ActionRepository repo, IPauseHandler pause)
        {
            _asset = asset;
            _repo = repo;
            _pause = pause;
        }

        public int CurrentIndex => _currentNodeIndex;

        public ValueTask MoveNextAsync()
        {
            _currentNodeIndex++;
            if (_currentNodeIndex >= _asset.Length)
            {
                return default;
            }

            if (TryCancel())
            {
                return default;
            }

            _cts = new CancellationTokenSource();
            ValueTask task = NextNodeAsync(_cts.Token);
            _task = task.AsTask();

            return task;
        }

        public bool TryCancel()
        {
            if (_cts == null)
            {
                return false;
            }

            if (_task != null && !_task.IsCompleted)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
                return true;
            }

            return false;
        }

        private readonly ScenarioDataAsset _asset;
        private readonly ActionRepository _repo;
        private readonly IPauseHandler _pause;

        private CancellationTokenSource _cts;
        private Task _task;
        private int _currentNodeIndex = -1;

        private async ValueTask NextNodeAsync(CancellationToken token = default)
        {
            ScenarioNode node = _asset[_currentNodeIndex];

            try
            {
                foreach(IScenarioAction action in node.ScenarioActions)
                {
                    await action.ExecuteAsync(_repo, _pause, token);
                }
            }
            catch(OperationCanceledException) { return; }

            return;
        }
    }
}
