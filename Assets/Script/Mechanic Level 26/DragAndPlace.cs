using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragAndPlaceUniversal : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform[] stepZones;
    public float snapDistance = 100f;

    static Dictionary<RectTransform, GameObject> occupied = new();
    RectTransform rect, snappedZone;
    Canvas canvas;
    bool isPlaced;

    void Awake() { rect = GetComponent<RectTransform>(); canvas = GetComponentInParent<Canvas>(); }

    public void OnBeginDrag(PointerEventData e)
    {
        if (!isPlaced) return;
        if (snappedZone && occupied.ContainsKey(snappedZone)) occupied.Remove(snappedZone);
        isPlaced = false; snappedZone = null;
    }

    public void OnDrag(PointerEventData e)
    {
        if (isPlaced) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, e.position, canvas.worldCamera, out var p);
        rect.anchoredPosition = p;
    }

    public void OnEndDrag(PointerEventData e)
    {
        if (isPlaced) return;
        RectTransform closest = null; float min = float.MaxValue;

        foreach (var z in stepZones)
        {
            float d = Vector2.Distance(rect.anchoredPosition, z.anchoredPosition);
            if (d < snapDistance && d < min) { min = d; closest = z; }
        }

        if (closest && !occupied.ContainsKey(closest))
        {
            rect.anchoredPosition = closest.anchoredPosition;
            occupied[closest] = gameObject;
            snappedZone = closest; isPlaced = true;
            ClimbManager.Instance?.RegisterStepFilled();
        }
    }
}
