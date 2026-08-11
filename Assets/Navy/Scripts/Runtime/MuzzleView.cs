using UnityEngine;

namespace NavyGame.Runtime
{
    public class MuzzleView : MonoBehaviour
    {
        public void PlayMuzzleFlash()
        {
            if (_muzzleFlashLight != null)
            {
                _muzzleFlashLight.enabled = true;
                Invoke(nameof(DisableMuzzleFlashLight), 0.05f);
            }
            if (_muzzleFlashParticle != null)
            {
                _muzzleFlashParticle.Play();
            }

            void DisableMuzzleFlashLight()
            {
                if (_muzzleFlashLight != null)
                {
                    _muzzleFlashLight.enabled = false;
                }
            }
        }

        [SerializeField]
        private Light _muzzleFlashLight;
        [SerializeField]
        private ParticleSystem _muzzleFlashParticle;
    }
}
