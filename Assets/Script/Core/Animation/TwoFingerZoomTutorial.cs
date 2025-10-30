using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TwoFingerZoomTutorial : MonoBehaviour
{
    [Header("Ngón tay trái & phải")]
    public RectTransform fingerLeft;
    public RectTransform fingerRight;

    [Header("Hiệu ứng")]
    public float moveDistance = 150f;   // khoảng cách 2 ngón tay di chuyển ra
    public float moveTime = 1.2f;       // thời gian di chuyển
    public float loopDelay = 0.6f;      // thời gian nghỉ giữa các lần
    public bool loopForever = true;     // lặp liên tục

    [Header("Nút X thoát hướng dẫn (tùy chọn)")]
    public Button closeButton;

    private Vector2 startLeftPos;
    private Vector2 startRightPos;
    private Sequence animSeq;

    void Start()
    {
        // Ghi nhớ vị trí ban đầu
        startLeftPos = fingerLeft.anchoredPosition;
        startRightPos = fingerRight.anchoredPosition;

        // Gắn nút X nếu có
        if (closeButton != null)
            closeButton.onClick.AddListener(StopTutorial);

        StartTutorial();
    }

    void OnDisable()
    {
        StopTutorial();
    }

    public void StartTutorial()
    {
        if (animSeq != null) animSeq.Kill();

        animSeq = DOTween.Sequence();

        // Hai ngón tay di chuyển ngược hướng nhau
        animSeq.Append(fingerLeft.DOAnchorPos(startLeftPos - Vector2.right * moveDistance, moveTime).SetEase(Ease.OutQuad));
        animSeq.Join(fingerRight.DOAnchorPos(startRightPos + Vector2.right * moveDistance, moveTime).SetEase(Ease.OutQuad));

        // Sau đó quay lại vị trí cũ
        animSeq.Append(fingerLeft.DOAnchorPos(startLeftPos, moveTime).SetEase(Ease.InOutQuad));
        animSeq.Join(fingerRight.DOAnchorPos(startRightPos, moveTime).SetEase(Ease.InOutQuad));

        if (loopForever)
            animSeq.AppendInterval(loopDelay).SetLoops(-1, LoopType.Restart);
    }

    public void StopTutorial()
    {
        if (animSeq != null)
        {
            animSeq.Kill();
            animSeq = null;
        }
        gameObject.SetActive(false);
    }
}
