using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HoldTutorial : MonoBehaviour
{
    [Header("Tham chiếu UI")]
    public RectTransform finger;         // Hình tay
    public RectTransform ripple;         // Hiệu ứng vòng sáng (tuỳ chọn)
    public Button closeButton;           // Nút X để thoát

    [Header("Cài đặt hiệu ứng")]
    public float pressScale = 0.9f;      // Tay ấn xuống
    public float holdTime = 1.2f;        // Thời gian giữ
    public float releaseScale = 1.05f;   // Nảy nhẹ khi thả
    public float loopDelay = 0.4f;       // Thời gian nghỉ giữa các vòng
    public float rippleScale = 2f;       // Vòng sáng lan to
    public float rippleFadeTime = 1f;    // Thời gian mờ dần

    private Sequence holdSeq;
    private Vector3 originalScale;

    void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(StopTutorial);

        StartTutorial();
    }

    void OnDisable()
    {
        if (holdSeq != null) holdSeq.Kill();
    }

    public void StartTutorial()
    {
        if (finger == null)
        {
            Debug.LogWarning("⚠️ Chưa gán finger!");
            return;
        }

        originalScale = finger.localScale;

        if (ripple != null)
        {
            ripple.localScale = Vector3.zero;
            var rippleImg = ripple.GetComponent<Image>();
            var c = rippleImg.color;
            c.a = 0f;
            rippleImg.color = c;
        }

        holdSeq = DOTween.Sequence();

        // ✋ Tay ấn xuống → giữ → nhả ra → lặp lại
        holdSeq.Append(finger.DOScale(pressScale, 0.25f).SetEase(Ease.OutQuad))
               .AppendInterval(holdTime)
               .Append(finger.DOScale(releaseScale, 0.25f).SetEase(Ease.OutBack))
               .Append(finger.DOScale(originalScale, 0.3f).SetEase(Ease.InOutQuad))
               .AppendInterval(loopDelay)
               .SetLoops(-1, LoopType.Restart);

        // 🌟 Hiệu ứng ripple sáng mỗi lần ấn
        if (ripple != null)
        {
            var rippleImg = ripple.GetComponent<Image>();
            holdSeq.Join(
                ripple.DOScale(rippleScale, rippleFadeTime)
                      .From(0.2f)
                      .SetEase(Ease.OutQuad)
                      .SetLoops(-1, LoopType.Restart)
            );
            holdSeq.Join(
                rippleImg.DOFade(0f, rippleFadeTime)
                         .From(1f)
                         .SetEase(Ease.OutQuad)
                         .SetLoops(-1, LoopType.Restart)
            );
        }
    }

    public void StopTutorial()
    {
        if (holdSeq != null)
        {
            holdSeq.Kill();
            holdSeq = null;
        }

        if (finger != null)
            finger.localScale = originalScale;

        gameObject.SetActive(false);
    }
}
