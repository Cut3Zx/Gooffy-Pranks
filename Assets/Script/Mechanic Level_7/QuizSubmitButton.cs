using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class QuizSubmitButton : BaseObjectManager
{
    [Header("References")]
    public TMP_InputField answerField;   // Ô nhập
    public TextMeshProUGUI feedbackText; // Text hiển thị phản hồi

    [Header("Answer")]
    public string correctAnswer = "2";   // Đáp án đúng
    public bool trimWhitespace = true;   // Cắt khoảng trắng
    public bool acceptLeadingZeros = true; // Cho phép "02" = "2"

    [Header("UI & Sound Effects (Optional)")]
    public GameObject correctFX;         // Hiệu ứng khi đúng
    public GameObject wrongFX;           // Hiệu ứng khi sai
    public AudioSource audioSource;      // Nguồn âm thanh
    public AudioClip sfxCorrect;
    public AudioClip sfxWrong;

    private bool solved = false;

    protected override void Awake()
    {
        base.Awake();

        if (answerField != null)
        {
            // Giới hạn chỉ cho nhập số
            answerField.contentType = TMP_InputField.ContentType.IntegerNumber;
            answerField.characterValidation = TMP_InputField.CharacterValidation.Integer;
        }

        // Gán sự kiện cho nút Xác nhận (nếu có)
        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(CheckAnswer);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        // Nếu không có Button component, vẫn xử lý được click
        CheckAnswer();
    }

    private void CheckAnswer()
    {
        if (solved || answerField == null) return;

        string userInput = answerField.text ?? "";

        if (trimWhitespace)
            userInput = userInput.Trim();

        if (acceptLeadingZeros)
        {
            if (int.TryParse(userInput, out int val))
                userInput = val.ToString();
        }

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
        {
            feedbackText.text = correct ? "✅ Chính xác!" : "❌ Sai rồi, thử lại nhé!";
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(correct ? sfxCorrect : sfxWrong);
        }

        if (correctFX != null) correctFX.SetActive(correct);
        if (wrongFX != null) wrongFX.SetActive(!correct);
    }
}
