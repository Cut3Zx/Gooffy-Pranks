using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject tutorialPanel;       // panel con trong canvas
    public RectTransform hand;             // hình bàn tay
    public Button closeButton;             // nút X

    [Header("Animation Settings")]
    public Vector2 moveOffset = new Vector2(200f, 0f); // khoảng di chuyển
    public float moveDuration = 1f;
    public Ease moveEase = Ease.InOutSine;

    private Tween handTween;
    private CanvasGroup panelGroup;

    void Awake()
    {
        // tự thêm CanvasGroup nếu chưa có (để fade)
        panelGroup = tutorialPanel.GetComponent<CanvasGroup>();
        if (panelGroup == null)
            panelGroup = tutorialPanel.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        ShowTutorial();
    }

    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
        panelGroup.alpha = 0f;
        panelGroup.DOFade(1f, 0.3f);

        if (hand != null)
        {
            Vector2 startPos = hand.anchoredPosition;
            Vector2 endPos = startPos + moveOffset;

            handTween = hand.DOAnchorPos(endPos, moveDuration)
                            .SetEase(moveEase)
                            .SetLoops(-1, LoopType.Yoyo);
        }

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(HideTutorial);
    }

    public void HideTutorial()
    {
        // Dừng animation tay
        if (handTween != null && handTween.IsActive())
            handTween.Kill();

        closeButton.onClick.RemoveAllListeners();

        // Ẩn panel mượt
        panelGroup.DOFade(0f, 0.3f)
            .OnComplete(() => tutorialPanel.SetActive(false));
    }
}
