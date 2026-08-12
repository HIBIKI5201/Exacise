namespace NavyGame.Runtime
{
    using UnityEngine;

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

            _turnVelocity += moveInput.x * _status.TurnAcceleration * deltaTime;
            _turnVelocity = Mathf.Lerp(_turnVelocity, 0f, _status.TurnDrag * deltaTime);
            _turnVelocity = Mathf.Clamp(_turnVelocity, -_status.MaxTurnSpeed, _status.MaxTurnSpeed);
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, _turnVelocity * deltaTime, 0);

            Vector3 thrust = transform.forward * moveInput.y * _status.Acceleration * deltaTime;
            _velocity += thrust;
            _velocity = Vector3.Lerp(_velocity, Vector3.zero, _status.MoveDrag * deltaTime);
            _velocity = Vector3.ClampMagnitude(_velocity, _status.MaxSpeed);

            transform.position += _velocity * deltaTime;
            transform.rotation = rotation;

            Debug.Log(moveInput);
        }

        private readonly ShipStatus _status;
        private readonly ShipViewContainer _container;

        private Vector3 _velocity = Vector3.zero;
        private float _turnVelocity = 0;
    }
}
