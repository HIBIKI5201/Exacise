using UnityEngine;

namespace NavyGame.Runtime
{
    public class ShipLifeCycle : ITickable
    {
        public ShipLifeCycle(InputBuffer input, ShipMover mover)
        {
            _input = input;
            _mover = mover;
        }

        public void Tick(float deltaTime)
        {
            Vector2 moveInput = _input.MoveInput;
            _mover.Tick(deltaTime, moveInput);
        }

        private readonly InputBuffer _input;
        private readonly ShipMover _mover;
    }
}
