using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GridDungeon.Scripts
{
    /// <summary>
    /// プレイヤーの入力を処理し、キャラクターを操作します。
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class CharacterController : MonoBehaviour
    {
        /// <summary>
        /// Input Action Assetで定義された移動アクションの名前。
        /// </summary>
        private const string MOVE_ACTION = "Move";

        [SerializeField, Tooltip("操作するキャラクター")]
        private CharacterMover _character;
        [SerializeField]
        private EnemyManager _enemyManager;

        private PlayerInput _playerInput;

        private bool _isMoving = false;

        private void OnEnable()
        {
            // コンポーネントの取得とnullチェック
            _playerInput = GetComponent<PlayerInput>();
            if (_character == null)
            {
                Debug.LogError("CharacterMoverが設定されていません。", this);
                return;
            }

            // PlayerInputのセットアップ
            if (_playerInput != null)
            {
                _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
                // イベントハンドラの登録
                var moveAction = _playerInput.actions[MOVE_ACTION];
                if (moveAction != null)
                {
                    moveAction.started += OnMove;
                }
            }
        }

        private void OnDisable()
        {
            // イベントハンドラの解除
            if (_playerInput != null)
            {
                var moveAction = _playerInput.actions[MOVE_ACTION];
                if (moveAction != null)
                {
                    moveAction.started -= OnMove;
                }
            }
        }

        /// <summary>
        /// 移動入力があったときに呼び出されます。
        /// </summary>
        /// <param name="context">入力アクションのコンテキスト</param>
        private async void OnMove(InputAction.CallbackContext context)
        {
            // CharacterMoverがなければ何もしない
            if (_character == null) return;
            if (_enemyManager == null) return;

            if (_isMoving) return; // すでに移動中なら無視
            _isMoving = true;

            // 入力ベクトルを読み取り、整数に変換して移動メソッドを呼び出す
            Vector2 inputVector = context.ReadValue<Vector2>();
            await _character.MoveTo(Vector2Int.CeilToInt(inputVector));

            await _enemyManager.MoveEnemies();

            _isMoving = false;
        }
    }
}