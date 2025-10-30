using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragAndSnap : BaseObjectManager
{
    [Header("Vùng gắn (Drop Zones)")]
    public RectTransform snapZone1;
    public RectTransform snapZone2;
    public float snapDistance = 100f;
    public bool lockAfterSnap = true;

    [Header("Âm Thanh Khi Chạm")]
    public string TouchSoundName;

    static Dictionary<RectTransform, GameObject> occupied = new();

    bool isSnapped = false;
    Vector2 startPos;

    public override void OnBeginDrag(PointerEventData e)
    {
        if (isSnapped) return;
        rectTransform ??= GetComponent<RectTransform>();
        canvas ??= GetComponentInParent<Canvas>();
        startPos = rectTransform.anchoredPosition; // 📍 lưu vị trí ban đầu
    }

    public override void OnDrag(PointerEventData e)
    {
        if (isSnapped || rectTransform == null || canvas == null) return;
        rectTransform.anchoredPosition += e.delta / canvas.scaleFactor;
    }

    public override void OnEndDrag(PointerEventData e)
    {
        if (isSnapped) return;

        RectTransform closest = GetClosestSnapZone();
        if (closest && !occupied.ContainsKey(closest))
        {
            rectTransform.anchoredPosition = closest.anchoredPosition;
            isSnapped = lockAfterSnap;
            occupied[closest] = gameObject;

            Debug.Log($"✅ {gameObject.name} đã gắn vào vùng {closest.name}");
            if (SFXManager.Instance != null && !string.IsNullOrEmpty(TouchSoundName))
            {
                SFXManager.Instance.PlaySFX(TouchSoundName);
            }
            SnapManager.Instance?.RegisterSnappedObject();
        }
        else
        {
            // ⏪ Không đúng vùng thì quay lại chỗ cũ
            rectTransform.anchoredPosition = startPos;
            Debug.Log("↩️ Không đặt đúng vị trí, quay lại chỗ cũ!");
        }
    }

    RectTransform GetClosestSnapZone()
    {
        RectTransform closest = null;
        float min = float.MaxValue;
        foreach (var z in new[] { snapZone1, snapZone2 })
        {
            if (z == null) continue;
            float d = Vector2.Distance(rectTransform.anchoredPosition, z.anchoredPosition);
            if (d < snapDistance && d < min) { min = d; closest = z; }
        }
        return closest;
    }
}
