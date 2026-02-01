using NovelGame.Master.Scripts.Infra;
using NovelGame.Master.Scripts.UI;
using NovelGame.Master.Scripts.UseCase;
using NovelGame.Master.Scripts.Utility;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NovelGame.Master.Scripts.Presenter
{
    public class ScenarioPlayer
    {
        public ScenarioPlayer(ScenarioDataAsset asset,
            ActionRepository repo,
            MessageWindowViewModel messageWindowViewModel,
            IPauseHandler ph)
        {
            _asset = asset;
            _repo = repo;
            _messageWindowViewModel = messageWindowViewModel;
            _ph = ph;
        }

        public int CurrentIndex => _currentNodeIndex;

        public ValueTask<bool> MoveNextAsync()
        {
            if (TryCancel())
            {
                return new(true);
            }

            _cts = new CancellationTokenSource();
            ValueTask<bool> task = NextNodeAsync(_cts.Token);
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
        private readonly MessageWindowViewModel _messageWindowViewModel;
        private readonly IPauseHandler _ph;

        private CancellationTokenSource _cts;
        private Task _task;
        private int _currentNodeIndex = -1;
        private List<ValueTask> _tasks = new();

        private async ValueTask<bool> NextNodeAsync(CancellationToken token = default)
        {
            ScenarioNode node = null;

            do
            {
                _currentNodeIndex++;

                if (_asset.Length <= _currentNodeIndex)
                {
                    return false;
                }

                node = _asset[_currentNodeIndex];

                ValueTask textTask = _messageWindowViewModel.SetTextAsync(node.Name, node.Text, token);
                _tasks.Add(textTask);

                foreach (IScenarioAction action in node.ScenarioActions)
                {
                    ValueTask task = action.ExecuteAsync(_repo, _ph, token);
                    _tasks.Add(task);
                }

                try
                {
                    foreach (ValueTask task in _tasks)
                    {
                        await task;
                    }
                }
                finally
                {
                    _tasks.Clear();
                }
            }
            while (node != null && !node.IsWaitForInput);

            return true;
        }
    }
}
