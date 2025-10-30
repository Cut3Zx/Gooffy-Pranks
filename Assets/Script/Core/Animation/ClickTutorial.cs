using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClickTutorial : MonoBehaviour
{
    [Header("References")]
    public RectTransform finger;       // Hình tay
    public RectTransform ripple;       // Vòng sáng
    public Button closeButton;         // Nút X để thoát

    [Header("Settings")]
    public float pressScale = 0.9f;    // Tay nhấn xuống
    public float rippleTime = 0.8f;    // Thời gian lan sáng
    public float loopDelay = 0.6f;     // Thời gian nghỉ
    public float fadeTime = 0.2f;      // Thời gian ẩn/hiện khi mở tutorial

    private Sequence loopSeq;

    void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(StopTutorial);
    }

    void OnEnable()
    {
        StartTutorial();
    }

    void OnDisable()
    {
        if (loopSeq != null) loopSeq.Kill();
    }

    public void StartTutorial()
    {
        // Reset trạng thái ban đầu
        finger.localScale = Vector3.one;
        ripple.localScale = Vector3.zero;
        var rippleImg = ripple.GetComponent<Image>();
        var c = rippleImg.color;
        c.a = 0f;
        rippleImg.color = c;

        // Hiệu ứng lặp
        loopSeq = DOTween.Sequence();

        loopSeq.Append(finger.DOScale(pressScale, 0.2f).SetEase(Ease.OutQuad))
               .Join(ripple.DOScale(2f, rippleTime).From(0.2f))
               .Join(rippleImg.DOFade(0f, rippleTime).From(1f))
               .Append(finger.DOScale(1f, 0.2f).SetEase(Ease.OutQuad))
               .AppendInterval(loopDelay)
               .SetLoops(-1, LoopType.Restart);
    }

    public void StopTutorial()
    {
        if (loopSeq != null)
            loopSeq.Kill();

        // Ẩn toàn bộ tutorial
        gameObject.SetActive(false);
    }

    // ⚡ Gọi khi người chơi click đúng
    public void OnPlayerClicked()
    {
        StopTutorial();
    }
}
