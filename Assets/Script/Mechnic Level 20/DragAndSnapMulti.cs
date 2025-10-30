using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragAndSnapMulti : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Vùng gắn (Drop Zones)")]
    public RectTransform[] snapZones;
    public float snapDistance = 100f;

    [Header("Tuỳ chọn")]
    public bool lockAfterSnap = true;
    public string TouchSoundName;

    private RectTransform rectTransform;
    private Canvas canvas;
    private bool isSnapped = false;

    // Danh sách vùng đã có vật thể gắn vào
    private static Dictionary<RectTransform, GameObject> occupiedZones = new Dictionary<RectTransform, GameObject>();

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
            // ❌ Nếu vùng này đã bị chiếm thì không cho gắn nữa
            if (occupiedZones.ContainsKey(closest))
            {
                Debug.LogWarning($"❌ {closest.name} đã có vật thể khác gắn vào!");
                return;
            }

            // ✅ Nếu vùng trống thì gắn vào
            rectTransform.anchoredPosition = closest.anchoredPosition;
            if (SFXManager.Instance != null && !string.IsNullOrEmpty(TouchSoundName))
            {
                SFXManager.Instance.PlaySFX(TouchSoundName);
            }
            isSnapped = lockAfterSnap;

            // Đánh dấu vùng này là đã bị chiếm
            occupiedZones[closest] = gameObject;

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

    private void OnDestroy()
    {
        // Nếu bị xoá thì giải phóng vùng
        foreach (var kvp in occupiedZones)
        {
            if (kvp.Value == gameObject)
            {
                occupiedZones.Remove(kvp.Key);
                break;
            }
        }
    }
}
