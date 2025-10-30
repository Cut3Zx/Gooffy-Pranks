using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class QuizSubmitButton : BaseObjectManager
{
    [Header("References")]
    public TMP_InputField answerField;
    public TextMeshProUGUI feedbackText;

    [Header("Answer Settings")]
    public string correctAnswer = "2";
    public bool trimWhitespace = true;
    public bool acceptLeadingZeros = true;

    [Header("Visual Feedback")]
    public GameObject correctMark;   // ✅ icon đúng
    public GameObject wrongMark;     // ❌ icon sai
    public float markShowTime = 1.5f; // thời gian hiện icon sai

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sfxCorrect;
    public AudioClip sfxWrong;

    private bool solved = false;

    protected override void Awake()
    {
        base.Awake();

        if (answerField != null)
        {
            answerField.contentType = TMP_InputField.ContentType.IntegerNumber;
            answerField.characterValidation = TMP_InputField.CharacterValidation.Integer;
        }

        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(CheckAnswer);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        CheckAnswer();
    }

    private void CheckAnswer()
    {
        if (solved || answerField == null) return;

        string userInput = answerField.text ?? "";

        if (trimWhitespace)
            userInput = userInput.Trim();

        if (acceptLeadingZeros && int.TryParse(userInput, out int val))
            userInput = val.ToString();

        bool isCorrect = userInput == correctAnswer;

        if (isCorrect)
        {
            solved = true;
            ShowFeedback(true);
            if (GameManager.Instance != null)
                GameManager.Instance.EndGame(true);
        }
        else
        {
            ShowFeedback(false);
        }
    }

    private void ShowFeedback(bool correct)
    {
        if (feedbackText != null)
            feedbackText.text = correct ? "✅ Chính xác!" : "❌ Sai rồi, thử lại nhé!";

        if (audioSource != null)
            audioSource.PlayOneShot(correct ? sfxCorrect : sfxWrong);

        // Hiển thị icon đúng / sai
        if (correctMark != null) correctMark.SetActive(correct);
        if (wrongMark != null)
        {
            wrongMark.SetActive(!correct);
            if (!correct)
                StartCoroutine(HideWrongMark());
        }
    }

    private IEnumerator HideWrongMark()
    {
        yield return new WaitForSeconds(markShowTime);
        if (wrongMark != null)
            wrongMark.SetActive(false);
    }
}
