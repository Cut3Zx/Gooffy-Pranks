using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndPlaceUniversal : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    [Header("Danh sách các vùng có thể đặt")]
    public RectTransform[] stepZones;
    public float snapDistance = 100f;

    private bool isPlaced = false;
    private RectTransform snappedZone;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlaced)
        {
            // Nếu muốn cho phép kéo ra lại, bỏ comment dòng dưới
            // ClimbManager.Instance.UnregisterStepFilled();
            // isPlaced = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlaced) return;

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out localPos
        );
        rectTransform.anchoredPosition = localPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isPlaced) return;

        RectTransform closestZone = GetClosestStepZone();

        if (closestZone != null)
        {
            rectTransform.anchoredPosition = closestZone.anchoredPosition;
            isPlaced = true;
            snappedZone = closestZone;

            ClimbManager.Instance?.RegisterStepFilled();
            Debug.Log($"✅ {name} đã được đặt vào bậc {closestZone.name}");
        }
        else
        {
            Debug.Log("❌ Không có bậc nào gần để gắn!");
        }
    }

    private RectTransform GetClosestStepZone()
    {
        RectTransform closest = null;
        float minDistance = float.MaxValue;

        foreach (var zone in stepZones)
        {
            float dist = Vector2.Distance(rectTransform.anchoredPosition, zone.anchoredPosition);
            if (dist < snapDistance && dist < minDistance)
            {
                minDistance = dist;
                closest = zone;
            }
        }

        return closest;
    }
}
