using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MagnifierRevealUI : BaseObjectManager
{
    [Header("Magnifier Sprites")]
    public Sprite normalMagnifier;        // Ảnh kính lúp trống
    public Sprite revealMagnifier;        // Ảnh kính lúp có kiến
    public float revealDuration = 3f;     // Thời gian hiện con kiến (giây)
    public RectTransform targetObject;    // Tảng đá (đối tượng cần soi)

    private Image image;
    private bool hasRevealed = false;

    protected override void Awake()
    {
        base.Awake();
        image = GetComponent<Image>();
        if (image != null && normalMagnifier != null)
            image.sprite = normalMagnifier;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        // Kéo dựa theo Canvas (giống BaseObjectManager)
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        ResetPosition();
    }

    private void Update()
    {
        if (hasRevealed || targetObject == null || rectTransform == null) return;

        // Kiểm tra va chạm 2 RectTransform trong Canvas
        if (RectOverlaps(rectTransform, targetObject))
        {
            hasRevealed = true;
            Debug.Log("🔍 Kính lúp soi trúng tảng đá!");
            StartCoroutine(RevealAnt());
        }
    }

    private bool RectOverlaps(RectTransform a, RectTransform b)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(b, a.position, canvas.worldCamera);
    }

    private IEnumerator RevealAnt()
    {
        if (image != null && revealMagnifier != null)
            image.sprite = revealMagnifier;

        yield return new WaitForSeconds(revealDuration);

        if (image != null && normalMagnifier != null)
            image.sprite = normalMagnifier;

        Debug.Log("🐜 Phát hiện kiến!");
        if (GameManager.Instance != null)
            GameManager.Instance.EndGame(true);
    }
}
