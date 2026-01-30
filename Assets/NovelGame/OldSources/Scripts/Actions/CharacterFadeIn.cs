using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Scripts
{
    public class CharacterFadeIn : CharacterActionBase
    {
        public CharacterFadeIn(float duration, string name) : base(name) 
        {
            _duration = duration;
        }

        protected override async Task Proccess(CharacterAnimator character, IPauseHandler ph, CancellationToken token)
        {
            await character.FadeIn(_duration, ph, token);
        }

        [SerializeField]
        private float _duration = 0.5f;
    }
}