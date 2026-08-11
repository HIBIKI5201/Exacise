using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IsPalindromer : MonoBehaviour
{
    [SerializeField]
    private Text _outputText;
    [SerializeField]
    private InputField _inputField;
    [SerializeField]
    private Button _button;

    [Space, SerializeField]
    private JudgeMode _judgeMode = JudgeMode.Normal;

    private bool _isTaskRunning;

    private enum JudgeMode
    {
        Normal,
        NonAlloc,
        Span,
    }

    private void Start()
    {
        if (_button == null)
        {
            Debug.LogError("Button reference is missing.");
            return;
        }
        if (_inputField == null)
        {
            Debug.LogError("InputField reference is missing.");
            return;
        }
        if (_outputText == null)
        {
            Debug.LogError("OutputText reference is missing.");
            return;
        }

        _button.onClick.AddListener(OnExcecuted);
    }

    private async void OnExcecuted()
    {
        if (_isTaskRunning) { return; }

        string input = _inputField.text;
        if (string.IsNullOrEmpty(input) || input.Length < 2)
        {
            _outputText.text = "Please enter at least two characters.";
            return;
        }

        try
        {
            _isTaskRunning = true;

            // モードに応じて回文判定を実行。
            bool result = _judgeMode switch
            {
                JudgeMode.Normal => IsPalindrome(input),
                JudgeMode.NonAlloc => IsPalindromeNonAlloc(input),
                JudgeMode.Span => IsPalindromeWithSpan(input),
                _ => throw new Exception()
            };

            string resultText = result ? "Palindrome" : "Not a palindrome";

            _outputText.text = string.Empty;
            await Awaitable.WaitForSecondsAsync(0.25f, destroyCancellationToken);
            _outputText.text = resultText;
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred: {ex.Message}");
            _outputText.text = "An error occurred. Please check the console for details.";
        }
        finally
        {
            _isTaskRunning = false;
        }
    }

    private bool IsPalindrome(string input)
    {
        if (input == null) { return false; }
        if (input.Length <= 1) { return false; }

        input = input.ToLower();

        int halfLength = input.Length / 2;

        for (int i = 0; i < halfLength; i++)
        {
            char first = input[i];
            char last = input[^(i + 1)];

            if (first != last)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsPalindromeNonAlloc(string input)
    {
        if (input == null) { return false; }
        if (input.Length <= 1) { return false; } // 一文字は回文外。

        const int UpperLowerDiff = 'a' - 'A'; // 大文字と小文字の差。

        int halfLength = input.Length / 2;

        for (int i = 0; i < halfLength; i++)
        {
            int diff = input[i] - input[^(i + 1)];
            diff = Math.Abs(diff); // 差に変換。

            if (diff != 0 && diff != UpperLowerDiff)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsPalindromeWithSpan(string input)
    {
        if (input == null) { return false; }
        if (input.Length <= 1) { return false; }

        ReadOnlySpan<char> original = input.ToLower().AsSpan();
        Span<char> reversed = stackalloc char[original.Length];

        original.CopyTo(reversed);
        reversed.Reverse();

        return original.SequenceEqual(reversed);
    }
}
