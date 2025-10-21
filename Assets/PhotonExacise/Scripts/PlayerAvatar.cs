using Fusion;
using UnityEngine;

namespace PhotonExacise.Scripts
{
    public class PlayerAvatar : NetworkBehaviour
    {
        // プレイヤー名のネットワークプロパティを定義する
        [Networked]
        public NetworkString<_16> NickName { get; set; }

        private NetworkCharacterController _characterController;
        private NetworkMecanimAnimator _networkAnimator;

        private CharacterEntity _charaEntity;
        private PlayerAvatarView _view;
        public override void Spawned()
        {
            _charaEntity = new ($"{this.Object.Id.Raw}", 100);
            CharacterUseCase.Register(_charaEntity);

            _characterController = GetComponent<NetworkCharacterController>();
            _networkAnimator = GetComponentInChildren<NetworkMecanimAnimator>();

            _view = GetComponent<PlayerAvatarView>();

            // 自身がアバターの権限を持っているなら、カメラの追従対象にする
            if (HasStateAuthority)
            {
                _view.MakeCameraTarget();
            }

            _charaEntity.OnHealthChanged += (c, m) => _view.SetNickName($"{c}/{m}");
        }

        public override void FixedUpdateNetwork()
        {
            // 移動
            var cameraRotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
            var inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            _characterController.Move(cameraRotation * inputDirection);

            // ダメージ
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CharacterUseCase.Instance.Attack(_charaEntity, 10);
            }

            // アニメーション
            var animator = _networkAnimator.Animator;
            var grounded = _characterController.Grounded;
            var vy = _characterController.Velocity.y;
            animator.SetFloat("Speed", _characterController.Velocity.magnitude);
            animator.SetBool("Jump", !grounded && vy > 4f);
            animator.SetBool("Grounded", grounded);
            animator.SetBool("FreeFall", !grounded && vy < -4f);
            animator.SetFloat("MotionSpeed", 1f);
        }
    }
}
