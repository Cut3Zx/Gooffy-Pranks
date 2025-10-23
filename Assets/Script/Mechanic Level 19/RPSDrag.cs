using UnityEngine;
using UnityEngine.EventSystems;

public class RPSDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3 startPos;
    private RPSDrag[] allRPSObjects;

    [Header("Khoảng cách va chạm UI")]
    public float overlapDistance = 120f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        startPos = rectTransform.anchoredPosition;
    }

    private void Start()
    {
        allRPSObjects = FindObjectsOfType<RPSDrag>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Không làm gì nếu object đã bị tắt
        if (!gameObject.activeInHierarchy) return;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy) return;

        foreach (var other in allRPSObjects)
        {
            if (other == this) continue;
            if (!other.gameObject.activeInHierarchy) continue;

            float distance = Vector2.Distance(rectTransform.anchoredPosition, other.rectTransform.anchoredPosition);
            if (distance <= overlapDistance)
            {
                // Gọi xử lý logic
                if (RPSManager.Instance != null)
                {
                    RPSManager.Instance.CheckWin(gameObject, other.gameObject);
                }
                return;
            }
        }

        // Nếu không chạm ai → quay lại chỗ cũ
        rectTransform.anchoredPosition = startPos;
    }
}
