using TMPro;
using Unity.Cinemachine;
using UnityEngine;

namespace PhotonExacise.Scripts
{
    public class PlayerAvatarView : MonoBehaviour
    {
        [SerializeField]
        private CinemachineCamera cinemachineCamera;
        [SerializeField]
        private TextMeshPro nameLabel;

        public void MakeCameraTarget()
        {
            cinemachineCamera.Priority.Value = 100;
        }

        public void SetNickName(string nickName)
        {
            nameLabel.text = nickName;
        }

        private void LateUpdate()
        {
            // プレイヤー名のテキストを、ビルボード（常にカメラ正面向き）にする
            nameLabel.transform.rotation = Camera.main.transform.rotation;
        }
    }
}
