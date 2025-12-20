using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Scripts
{
    public class CharacterFadeOut : CharacterActionBase
    {
        protected override async Task Proccess(CharacterAnimator character, IPauseHandler ph, CancellationToken token)
        {
            await character.FadeOut(_duration, ph, token);
        }


        [SerializeField]
        private float _duration = 0.5f;
    }
}