using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShakeTutorial : MonoBehaviour
{
    [Header("Hình ảnh hướng dẫn lắc (ví dụ: điện thoại, mũi tên...)")]
    public RectTransform phoneIcon;     // biểu tượng điện thoại
    public float shakeAngle = 15f;      // góc lắc
    public float shakeSpeed = 0.3f;     // tốc độ lắc
    public float pauseTime = 0.2f;      // dừng lại giữa các lần lắc

    [Header("Hiệu ứng phụ (nếu có)")]
    public RectTransform rippleEffect;  // vòng sáng phụ (tùy chọn)
    public float rippleScale = 1.4f;
    public float rippleTime = 0.8f;

    [Header("Nút X để thoát hướng dẫn")]
    public Button closeButton;

    private Sequence shakeSeq;
    private Vector3 originalRot;

    void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(StopTutorial);

        StartTutorial();
    }

    void OnDisable()
    {
        if (shakeSeq != null) shakeSeq.Kill();
    }

    public void StartTutorial()
    {
        if (phoneIcon == null)
        {
            Debug.LogWarning("⚠️ Chưa gán phoneIcon!");
            return;
        }

        originalRot = phoneIcon.localEulerAngles;

        shakeSeq = DOTween.Sequence();

        // 🔄 Hiệu ứng lắc trái phải liên tục
        shakeSeq.Append(phoneIcon.DOLocalRotate(new Vector3(0, 0, shakeAngle), shakeSpeed).SetEase(Ease.InOutSine))
                .Append(phoneIcon.DOLocalRotate(new Vector3(0, 0, -shakeAngle), shakeSpeed).SetEase(Ease.InOutSine))
                .Append(phoneIcon.DOLocalRotate(originalRot, shakeSpeed).SetEase(Ease.InOutSine))
                .AppendInterval(pauseTime)
                .SetLoops(-1, LoopType.Restart);

        // 🌟 Hiệu ứng ripple sáng (tùy chọn)
        if (rippleEffect != null)
        {
            var img = rippleEffect.GetComponent<Image>();
            shakeSeq.Join(
                rippleEffect.DOScale(rippleScale, rippleTime).From(0.2f).SetEase(Ease.OutQuad)
            );
            shakeSeq.Join(
                img.DOFade(0f, rippleTime).From(1f).SetEase(Ease.OutQuad)
            );
        }
    }

    public void StopTutorial()
    {
        if (shakeSeq != null)
        {
            shakeSeq.Kill();
            shakeSeq = null;
        }

        if (phoneIcon != null)
            phoneIcon.localEulerAngles = originalRot;

        gameObject.SetActive(false);
    }
}
