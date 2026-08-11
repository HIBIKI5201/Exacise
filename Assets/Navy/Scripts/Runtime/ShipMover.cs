using UnityEngine;

namespace NavyGame.Runtime
{
    public class ShipMover
    {
        public ShipMover(ShipStatus status, ShipViewContainer container)
        {
            _status = status;
            _container = container;
        }

        public void Tick(float deltaTime, Vector2 moveInput)
        {
            Transform transform = _container.transform;
            Vector3 position = transform.position;

            _velocity += Vector3.forward * moveInput.y * _status.Acceleration * deltaTime;
            _velocity = Vector3.ClampMagnitude(_velocity, _status.MaxSpeed);
            position += _velocity * deltaTime;

            Quaternion rotation = transform.rotation;
            rotation *= Quaternion.Euler(0, moveInput.x * _status.TurnSpeed * deltaTime, 0);

            _container.transform.position = position;
            _container.transform.rotation = rotation;
        }

        private readonly ShipStatus _status;
        private readonly ShipViewContainer _container;

        private Vector3 _velocity = Vector3.zero;

    }
}
