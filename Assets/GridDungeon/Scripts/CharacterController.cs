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
        private const string MOVE_ACTION = "Move";
        private const string ATTACK_ACTION = "Jump";
        private const string STAY_ACTION = "Sprint";

        [SerializeField, Tooltip("操作するキャラクター")]
        private GameObject _player;
        [SerializeField]
        private EnemyManager _enemyManager;
        [SerializeField]
        private GridManager _gridManager;

        [SerializeField]
        private Vector3 _headOffset = new(0, 1.5f, 0.5f);
        [SerializeField]
        private float _headSize = 0.3f;

        [Space]
        [SerializeField, Min(1)]
        private int _attackDistance = 2;
        [SerializeField]
        private float _attackDelay = 0.5f;

        [SerializeField]
        private Vector3 _bulletOffset = new(0, 1, 0);

        private PlayerInput _playerInput;
        private CharacterMover _playerMover;
        private CharacterBattler _playerBattler;

        private bool _isControlable = true;

        private Vector2Int _playerDir;

        private bool _isStaying = false;

        private GameObject _bulletObj;

        private void Awake()
        {
            if (_player != null)
            {
                _playerMover = _player.GetComponent<CharacterMover>();
                _playerBattler = _player.GetComponent<CharacterBattler>();

                Transform head = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;

                head.parent = _player.transform;
                head.localPosition = _headOffset;
                head.localScale = Vector3.one * _headSize;
            }

            if (_bulletObj == null)
            {
                _bulletObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _bulletObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.5f);
                _bulletObj.GetComponent<Collider>().enabled = false;
                _bulletObj.SetActive(false);
            }

            _playerInput = GetComponent<PlayerInput>();

            _isControlable = true;
        }
        private void OnEnable()
        {
            // PlayerInputのセットアップ
            if (_playerInput != null)
            {
                _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
                // イベントハンドラの登録
                InputAction moveAction = _playerInput.actions[MOVE_ACTION];
                InputAction attackAction = _playerInput.actions[ATTACK_ACTION];
                InputAction stayAction = _playerInput.actions[STAY_ACTION];
                if (moveAction != null)
                {
                    moveAction.started += HandleMove;
                    attackAction.started += HanldeAttack;
                    stayAction.started += HanldeStay;
                    stayAction.canceled += HanldeStay;
                }
            }
        }

        private void OnDisable()
        {
            // イベントハンドラの解除
            if (_playerInput != null)
            {
                InputAction moveAction = _playerInput.actions[MOVE_ACTION];
                InputAction attackAction = _playerInput.actions[ATTACK_ACTION];
                InputAction stayAction = _playerInput.actions[STAY_ACTION];
                if (moveAction != null)
                {
                    moveAction.started -= HandleMove;
                    attackAction.started -= HanldeAttack;
                    stayAction.started -= HanldeStay;
                }
            }
        }

        /// <summary>
        /// 移動入力があったときに呼び出されます。
        /// </summary>
        /// <param name="context">入力アクションのコンテキスト</param>
        private async void HandleMove(InputAction.CallbackContext context)
        {
            // CharacterMoverがなければ何もしない
            if (_player == null) return;
            if (_enemyManager == null) return;

            if (!_isControlable) return; // すでに移動中なら無視
            _isControlable = false;

            // 入力ベクトルを読み取り、整数に変換して移動メソッドを呼び出す
            Vector2 inputVector = context.ReadValue<Vector2>();

            FlipPlayerDirection(inputVector);

            if (_isStaying) // その場に留まる場合
            {
                _isControlable = true;
                return;
            }

            bool isSuccess = await _playerMover.MoveTo(Vector2Int.CeilToInt(inputVector));

            if (!isSuccess)
            {
                _isControlable = true;
                return;
            }

            await EnemyTurn();

            _isControlable = true;
        }

        /// <summary>
        ///     攻撃入力があったときに呼び出されます。
        /// </summary>
        /// <param name="context"></param>
        private async void HanldeAttack(InputAction.CallbackContext context)
        {
            if (_enemyManager == null) return;

            if (!_isControlable) return;
            _isControlable = false;

            Vector2Int pos = _playerMover.Position;
            Vector2Int dir = _playerDir;



            ShootBullet(_player.transform.position,
                _player.transform.position + new Vector3(dir.x, 0, dir.y) * _attackDistance,
                _attackDelay);

            for (int i = 1; i <= _attackDistance; i++)
            {
                Vector2Int targetPos = pos + dir * i;

                CharacterBattler enemy = _enemyManager.GetEnemyAtPosition(targetPos);

                if (enemy != null)
                {
                    _playerBattler.Attack(enemy);
                    await Awaitable.WaitForSecondsAsync(_attackDelay);
                }
            }

            await EnemyTurn();

            _isControlable = true;
        }

        private void HanldeStay(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _isStaying = true;

            }
            else if (context.canceled)
            {
                _isStaying = false;
            }
        }

        /// <summary>
        ///     プレイヤーの向きを変更します。
        /// </summary>
        /// <param name="dir"></param>
        private void FlipPlayerDirection(Vector2 dir)
        {
            _playerDir = ToCardinalDirection(dir);

            if (_player == null) return;

            _player.transform.rotation = 
                Quaternion.LookRotation(new Vector3(_playerDir.x, 0, _playerDir.y));
        }

        private async Task EnemyTurn()
        {
            await _enemyManager.MoveEnemies();
        }

        /// <summary>
        ///     弾丸の発射をシミュレートします。
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="targetPos"></param>
        /// <param name="duration"></param>
        private async void ShootBullet(Vector3 startPos, Vector3 targetPos, float duration)
        {
            if (_bulletObj == null) return;
            Vector3 direction = (targetPos - startPos).normalized;

            _bulletObj.transform.position = startPos + _bulletOffset;
            _bulletObj.transform.rotation = Quaternion.LookRotation(direction);
            _bulletObj.SetActive(true);

            if (duration <= 0)
            {
                await Awaitable.NextFrameAsync();
                _bulletObj.SetActive(false);
                return;
            }

            float t = 0;

            while (t < 1)
            {
                t += Time.deltaTime / duration;
                Vector3 pos = Vector3.Lerp(startPos, targetPos, t) + _bulletOffset;
                _bulletObj.transform.position = pos;
                await Awaitable.NextFrameAsync();
            }

            _bulletObj.SetActive(false);
        }

        /// <summary>
        /// Vector2 を上下左右のいずれかの Vector2Int に変換する
        /// </summary>
        private Vector2Int ToCardinalDirection(Vector2 vec)
        {
            // 上下左右の候補
            Vector2Int[] directions = new Vector2Int[]
            {
            Vector2Int.up,    // (0, 1)
            Vector2Int.down,  // (0, -1)
            Vector2Int.left,  // (-1, 0)
            Vector2Int.right  // (1, 0)
            };

            // 最大の内積を探す（ベクトルの近さを判定）
            float maxDot = float.NegativeInfinity;
            Vector2Int bestDir = Vector2Int.zero;

            foreach (var dir in directions)
            {
                float dot = Vector2.Dot(vec.normalized, dir); // 正規化して比較
                if (dot > maxDot)
                {
                    maxDot = dot;
                    bestDir = dir;
                }
            }

            return bestDir;
        }
    }
}