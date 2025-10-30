using UnityEngine;
using UnityEngine.EventSystems;

public class AllowCorrectClick : MonoBehaviour, IPointerClickHandler
{
    [Header("Hiệu ứng khi click đúng")]
    [Tooltip("Prefab của dấu V hiển thị khi click đúng (phải là UI prefab có RectTransform).")]
    public GameObject correctMarkPrefab;

    [Tooltip("Canvas chính để spawn dấu V.")]
    public Canvas targetCanvas;

    [Tooltip("Tự động xoá dấu V sau vài giây (0 = giữ lại).")]
    public float autoHideTime = 1.5f;

    private GameObject spawnedMark;

    private void Awake()
    {
        // Nếu chưa gán thì tự tìm Canvas trong scene
        if (targetCanvas == null)
            targetCanvas = FindObjectOfType<Canvas>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (correctMarkPrefab == null)
        {
            Debug.LogWarning("⚠️ Chưa gán prefab dấu V!");
            return;
        }

        // Nếu đã có dấu V cũ thì xoá
        if (spawnedMark != null)
            Destroy(spawnedMark);

        // Tạo dấu V trong Canvas
        spawnedMark = Instantiate(correctMarkPrefab, targetCanvas.transform);

        RectTransform markRect = spawnedMark.GetComponent<RectTransform>();

        // 👉 Lấy toạ độ click (UI local position)
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetCanvas.transform as RectTransform,
            eventData.position,
            targetCanvas.worldCamera,
            out localPos
        );

        markRect.anchoredPosition = localPos;
        markRect.localScale = Vector3.one * 1f; // đảm bảo kích thước chuẩn

        // Luôn hiện lên trên cùng
        spawnedMark.transform.SetAsLastSibling();

        // Tự động xoá nếu có thời gian đặt
        if (autoHideTime > 0)
            Destroy(spawnedMark, autoHideTime);
    }
}
