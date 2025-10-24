using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndSnapMulti : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Vùng gắn (Drop Zones)")]
    public RectTransform[] snapZones; // cho phép nhiều vùng
    public float snapDistance = 100f;

    [Header("Tuỳ chọn")]
    public bool lockAfterSnap = true;

    private RectTransform rectTransform;
    private Canvas canvas;
    private bool isSnapped = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSnapped) return;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSnapped) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isSnapped) return;

        RectTransform closest = GetClosestSnapZone();

        if (closest != null)
        {
            rectTransform.anchoredPosition = closest.anchoredPosition;
            isSnapped = lockAfterSnap;
            Debug.Log($"✅ {gameObject.name} đã gắn vào vùng {closest.name}");

            WeighSnapManager.Instance?.RegisterSnappedObject();
        }
    }

    private RectTransform GetClosestSnapZone()
    {
        RectTransform closest = null;
        float minDistance = float.MaxValue;

        foreach (RectTransform zone in snapZones)
        {
            if (zone == null) continue;
            float distance = Vector2.Distance(rectTransform.anchoredPosition, zone.anchoredPosition);
            if (distance < snapDistance && distance < minDistance)
            {
                minDistance = distance;
                closest = zone;
            }
        }

        return closest;
    }
}
