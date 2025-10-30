using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragAndPlaceUniversal : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Các vùng có thể gắn")]
    public RectTransform[] stepZones;
    public float snapDistance = 100f;

    static Dictionary<RectTransform, GameObject> occupied = new();

    private RectTransform rect;
    private Canvas canvas;
    private bool isPlaced = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        // ❌ Nếu đã đặt rồi thì không cho kéo nữa
        if (isPlaced) return;
    }

    public void OnDrag(PointerEventData e)
    {
        if (isPlaced) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, e.position, canvas.worldCamera, out var localPos);

        rect.anchoredPosition = localPos;
    }

    public void OnEndDrag(PointerEventData e)
    {
        if (isPlaced) return;

        RectTransform closest = null;
        float minDist = float.MaxValue;

        foreach (var zone in stepZones)
        {
            float dist = Vector2.Distance(rect.anchoredPosition, zone.anchoredPosition);
            if (dist < snapDistance && dist < minDist)
            {
                minDist = dist;
                closest = zone;
            }
        }

        // ✅ Gắn vào vùng trống gần nhất
        if (closest != null && !occupied.ContainsKey(closest))
        {
            rect.anchoredPosition = closest.anchoredPosition;
            occupied[closest] = gameObject;
            isPlaced = true; // 🔒 Cố định – không kéo lại được
            ClimbManager.Instance?.RegisterStepFilled();
        }
    }
}
