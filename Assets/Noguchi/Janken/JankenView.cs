using DG.Tweening;
using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JankenView : MonoBehaviour, IDisposable
{
    public event Action<JankenHandEnum> BattleStarted;

    public void SetResult(JankenRequester.Result result)
    {
        _self.sprite = GetJankenSprite(result.Self);
        _opponent.sprite = GetJankenSprite(result.Opponent);
        _text.text = GetJankenResultMessage(result.ResultType);

        _buttonGroup.interactable = true;
    }

    public async ValueTask BattleStartPerformance(CancellationToken token = default)
    {

        _animationText.text = "じゃん";

        ValueTask t = TextAnime();
        ValueTask o = OpponentSpriteAnime();

        await t;
        await o;

        async ValueTask TextAnime()
        {
            _animationText.transform.localScale = Vector3.one;
            await _animationText.transform.DOScale(new Vector3(1.3f, 1.3f),
                ANIMATION_DURATION / 3)
                .AsyncWaitForCompletion();

            _animationText.text = "けん";
            _animationText.transform.localScale = Vector3.one;
            await _animationText.transform.DOScale(new Vector3(1.3f, 1.3f),
                ANIMATION_DURATION / 3)
                .AsyncWaitForCompletion();

            _animationText.text = "ぽん";
            _animationText.transform.localScale = Vector3.one;
            _animationText.transform.DOScale(new Vector3(1.3f, 1.3f),
                ANIMATION_DURATION / 3);
        }

        async ValueTask OpponentSpriteAnime()
        {
            float t = ANIMATION_DURATION * 2 / 3;
            int i = 0;
            do
            {
                float d = ANIMATION_DURATION / 30;

                _opponent.sprite = GetJankenSprite((JankenHandEnum)(i % 3) + 1);

                await Awaitable.WaitForSecondsAsync(d);
                t -= d;
                i++;
            }
            while (0 < t);
        }
    }

    public void Dispose()
    {
        _rButton.onClick.RemoveAllListeners();
        _sButton.onClick.RemoveAllListeners();
        _pButton.onClick.RemoveAllListeners();
    }

    private const float ANIMATION_DURATION = 2f;

    [SerializeField]
    private TextMeshProUGUI _animationText;

    [Header("Selector")]
    [SerializeField]
    private CanvasGroup _buttonGroup;
    [SerializeField]
    private Button _rButton;
    [SerializeField]
    private Button _sButton;
    [SerializeField]
    private Button _pButton;

    [Header("Result")]
    [SerializeField]
    private Image _self;
    [SerializeField]
    private Image _opponent;


    [Header("Sprite")]

    [SerializeField]
    private Sprite _rock;
    [SerializeField]
    private Sprite _scissors;
    [SerializeField]
    private Sprite _paper;

    [SerializeField]
    private TextMeshProUGUI _text;

    private JankenHandEnum _selectHand = JankenHandEnum.None;

    private void Start()
    {
        _rButton.onClick.AddListener(() => SelectorClickedHandler(JankenHandEnum.Rock));
        _sButton.onClick.AddListener(() => SelectorClickedHandler(JankenHandEnum.Scissors));
        _pButton.onClick.AddListener(() => SelectorClickedHandler(JankenHandEnum.Paper));
    }

    private void SelectorClickedHandler(JankenHandEnum e)
    {
        _selectHand = e;
        _self.sprite = GetJankenSprite(e);
        bool r = InvokeBattleStart();
        if (!r) { return; }
        _buttonGroup.interactable = false;
    }

    private bool InvokeBattleStart()
    {
        if (_selectHand == JankenHandEnum.None)
        {
            _text.text = "手を選んでください";
            return false;
        }

        BattleStarted?.Invoke(_selectHand);
        return true;
    }

    private Sprite GetJankenSprite(JankenHandEnum e)
    {
        return e switch
        {
            JankenHandEnum.Rock => _rock,
            JankenHandEnum.Scissors => _scissors,
            JankenHandEnum.Paper => _paper,
            _ => null
        };
    }

    private string GetJankenResultMessage(JankenResultEnum e)
    {
        return e switch
        {
            JankenResultEnum.Draw => "引き分け！",
            JankenResultEnum.Win => "勝ち！",
            JankenResultEnum.Lose => "負け！",
            _ => "エラー"
        };
    }
}
