using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndSnap : BaseObjectManager
{
    [Header("Vùng gắn (Drop Zones)")]
    public RectTransform snapZone1;
    public RectTransform snapZone2;
    public float snapDistance = 100f;

    [Header("Tuỳ chọn")]
    public bool lockAfterSnap = true;

    private bool isSnapped = false;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (isSnapped) return;

        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        if (canvas == null) canvas = GetComponentInParent<Canvas>();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (isSnapped || rectTransform == null || canvas == null) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (isSnapped) return;

        RectTransform closestZone = GetClosestSnapZone();
        if (closestZone != null)
        {
            rectTransform.anchoredPosition = closestZone.anchoredPosition;
            isSnapped = true;
            Debug.Log($"✅ {gameObject.name} đã gắn vào vùng {closestZone.name}");

            // 🔔 Báo về Manager
            SnapManager.Instance?.RegisterSnappedObject();
        }
    }

    private RectTransform GetClosestSnapZone()
    {
        RectTransform closest = null;
        float minDistance = float.MaxValue;

        if (snapZone1 != null)
        {
            float dist1 = Vector2.Distance(rectTransform.anchoredPosition, snapZone1.anchoredPosition);
            if (dist1 < snapDistance && dist1 < minDistance)
            {
                minDistance = dist1;
                closest = snapZone1;
            }
        }

        if (snapZone2 != null)
        {
            float dist2 = Vector2.Distance(rectTransform.anchoredPosition, snapZone2.anchoredPosition);
            if (dist2 < snapDistance && dist2 < minDistance)
            {
                minDistance = dist2;
                closest = snapZone2;
            }
        }

        return closest;
    }
}
