using UnityEngine;
using UnityEngine.EventSystems;

public class RakeDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 startPos;

    [Header("References")]
    public GameObject hay;    // đống lúa
    public GameObject chick;  // gà con

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        startPos = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling(); // Đưa cào lên trên cùng (tránh bị che)
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Di chuyển theo chuột/touch trong canvas
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (hay == null || chick == null) return;

        RectTransform hayRect = hay.GetComponent<RectTransform>();

        // Kiểm tra xem cào có thả trúng đống lúa không
        if (RectTransformUtility.RectangleContainsScreenPoint(hayRect, Input.mousePosition, canvas.worldCamera))
        {
            Debug.Log("🧹 Cào chạm đống lúa — Gà con xuất hiện!");
            hay.SetActive(false);
            chick.SetActive(true);
        }

        // Trả cào về vị trí cũ
        rectTransform.anchoredPosition = startPos;
    }
}
