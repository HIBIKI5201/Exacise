using UnityEngine;

namespace NavyGame.Runtime
{
    public class TargetContainer : MonoBehaviour
    {
        public Transform[] Targets => _targets;

        [SerializeField]
        private Transform[] _targets;
    }
}
