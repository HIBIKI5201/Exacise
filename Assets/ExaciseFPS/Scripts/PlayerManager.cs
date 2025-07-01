using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ExaciseFPS.SourceCode
{
    /// <summary>
    ///     プレイヤーのマネージャー
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private CinemachineCamera _camera;

        [SerializeField]
        private Transform _head;
        
        private void Start()
        {
            if (_camera == null)
            {
                _camera = FindAnyObjectByType<CinemachineCamera>();
            }
        }

        private void Update()
        {
            CameraPositionLink();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
        }

        private void CameraPositionLink()
        {
            //カメラの位置にプレイヤーが同期する
            if (_camera != null && _head != null)
            {
                transform.position = _camera.transform.position - _head.localPosition;
            }
        }
    }
}