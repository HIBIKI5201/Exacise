using UnityEngine;
using UnityEngine.InputSystem;

namespace ExaciseFPS.SourceCode
{
    /// <summary>
    ///   カメラのマネージャー
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        public void RegisterInputAction()
        {
            if (_inputBuffer != null)
            {
                _inputBuffer.MoveAction.performed += OnInputMove;
                _inputBuffer.MoveAction.canceled += OnInputMove;
                _inputBuffer.LookAction.performed += OnInputLook;
                _inputBuffer.LookAction.canceled += OnInputLook;
                _inputBuffer.AttackAction.started += OnInputAttack;
            }
        }

        [SerializeField]
        private float _moveSpeed = 5f;
        [SerializeField]
        private float _lookSpeed = 2f;

        [SerializeField]
        private float _attackPower = 10f;

        private InputBuffer _inputBuffer;

        private Vector3 _velocity = Vector3.zero;
        private Vector2 _lookAxis = Vector2.zero;

        private void Start()
        {
            _inputBuffer = FindAnyObjectByType<InputBuffer>();
            RegisterInputAction();

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // 移動速度が0の場合は何もしない
            if (Mathf.Approximately(_velocity.magnitude, 0)) return;

            // カメラの位置を更新
            transform.position += _velocity * Time.deltaTime;
        }

        private void OnInputMove(InputAction.CallbackContext context)
        {
            Vector2 dir = context.ReadValue<Vector2>().normalized;

            //ローカル座標系での移動方向を計算
            Vector3 localDir =
                transform.InverseTransformDirection(new Vector3(dir.x, 0, dir.y));

            _velocity = localDir * _moveSpeed;
        }

        private void OnInputLook(InputAction.CallbackContext context)
        {
            Vector2 dir = context.ReadValue<Vector2>().normalized;

            _lookAxis.x += dir.x * _lookSpeed * Time.deltaTime;
            _lookAxis.y -= dir.y * _lookSpeed * Time.deltaTime;

            _lookAxis.x = Mathf.Repeat(_lookAxis.x, 360f);
            _lookAxis.y = Mathf.Clamp(_lookAxis.y, -90f, 90f);

            transform.rotation = Quaternion.Euler(_lookAxis.y, _lookAxis.x, 0);
        }

        private void OnInputAttack(InputAction.CallbackContext context)
        {
            if (Physics.Raycast(transform.position, transform.forward, out var hit, 100f))
            {
                if (hit.rigidbody?.TryGetComponent<IHitable>(out var target) ?? false)
                {
                    target.Hit(_attackPower);
                }
            }
        }
    }
}
