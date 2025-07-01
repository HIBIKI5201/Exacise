using UnityEngine;
using UnityEngine.InputSystem;

namespace ExaciseFPS.SourceCode
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputBuffer : MonoBehaviour
    {
        private const string MOVE_ACTION = "Move";
        private const string LOOK_ACTION = "Look";
        private const string ATTACK_ACTION = "Attack";

        public InputAction MoveAction => _moveAction;
        public InputAction LookAction => _lookAction;
        public InputAction AttackAction => _attackAction;

        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _attackAction;

        private void Awake()
        {
            //ƒAƒNƒVƒ‡ƒ“‚ðŽæ“¾
            if (TryGetComponent<PlayerInput>(out var playerInput))
            {
                _moveAction = playerInput.actions[MOVE_ACTION];
                _lookAction = playerInput.actions[LOOK_ACTION];
                _attackAction = playerInput.actions[ATTACK_ACTION];
            }
        }
    }
}
