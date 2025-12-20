using UnityEngine;
using UnityEngine.InputSystem;

namespace AbeShootingGame
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        [Space]

        [SerializeField]
        private string _moveName = string.Empty;
        [SerializeField]
        private string _attackName = string.Empty;

        [Space]

        [SerializeField]
        private BulletContainer _bulletContainer;

        private PlayerInput _playerInput;
        private Vector2 _moveDirection;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            Register(_playerInput);
        }

        private void Update()
        {
            UpdateMove(_moveDirection);
        }

        private void Register(PlayerInput input)
        {
            if (input == null) { return; }

            InputAction moveAction = input.actions[_moveName];
            if (moveAction != null)
            {
                moveAction.performed += HandleMove;
                moveAction.canceled += HandleMove;

                destroyCancellationToken.Register(() =>
                {
                    moveAction.performed -= HandleMove;
                    moveAction.canceled -= HandleMove;
                });
            }
            else { Debug.LogError("Moveがありません"); }

            InputAction attackAction = input.actions[_attackName];
            if (attackAction != null)
            {
                attackAction.started += HandleAttack;

                destroyCancellationToken.Register(() =>
                {
                    attackAction.started -= HandleAttack;
                });
            }
            else { Debug.LogError("Attackがありません"); }
        }

        private void HandleMove(InputAction.CallbackContext context)
        {
            _moveDirection = context.ReadValue<Vector2>();
        }

        private void HandleAttack(InputAction.CallbackContext context)
        {
            _bulletContainer.Fire(transform.position);
        }

        private void UpdateMove(Vector2 direction)
        {
            Vector2 velocity = direction * Time.deltaTime * _speed;
            transform.position += new Vector3(velocity.x, velocity.y);
        }
    }


}