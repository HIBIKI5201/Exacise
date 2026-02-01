using NovelGame.Master.Scripts.Utility;

namespace NovelGame.Master.Scripts.UI
{
    public class PauseManager : IPauseHandler
    {
        public PauseManager(params IPauseHandler[] pauseHandlers)
        {
            _handlers = pauseHandlers;
        }

        public bool IsPaused
        {
            get
            {
                if (_handlers.Length <= 0) { return false; }

                foreach (IPauseHandler handler in _handlers)
                {
                    if (handler.IsPaused) { return true; }
                }

                return false;
            }
        }

        private IPauseHandler[] _handlers;
    }
}
