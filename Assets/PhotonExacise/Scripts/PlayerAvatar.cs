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
        public override void Spawned()
        {
            _characterController = GetComponent<NetworkCharacterController>();
            _networkAnimator = GetComponentInChildren<NetworkMecanimAnimator>();

            var view = GetComponent<PlayerAvatarView>();

            // プレイヤー名をテキストに反映する
            view.SetNickName(NickName.Value);

            // 自身がアバターの権限を持っているなら、カメラの追従対象にする
            if (HasStateAuthority)
            {
                view.MakeCameraTarget();
            }
        }

        public override void FixedUpdateNetwork()
        {
            // 移動
            var cameraRotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
            var inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            _characterController.Move(cameraRotation * inputDirection);

            // ジャンプ
            if (Input.GetKey(KeyCode.Space))
            {
                _characterController.Jump();
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
