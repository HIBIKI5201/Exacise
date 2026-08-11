using UnityEngine;
using UnityEngine.InputSystem;

namespace NavyGame.Runtime
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputBuffer : MonoBehaviour
    {
        public Vector2 MoveInput => _moveInput;

        [SerializeField, Tooltip("移動のアクション名")]
        private string _moveActionName = "Move";

        private Vector2 _moveInput;

        private void Start()
        {
            PlayerInput playerInput = GetComponent<PlayerInput>();
            if (playerInput == null) { return; }

            playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;

            InputAction moveAction = playerInput.actions[_moveActionName];
            moveAction.performed += MoveHandler;
        }

        private void MoveHandler(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }
    }
}
