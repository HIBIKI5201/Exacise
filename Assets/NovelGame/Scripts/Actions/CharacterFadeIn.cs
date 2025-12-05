using System.Threading.Tasks;

namespace NovelGame.Scripts.Actions
{
    public class CharacterFadeIn : CharacterActionBase
    {
        protected override async Task Proccess(CharacterAnimator character)
        {
            await character.FadeIn();
        }
    }
}