using UnityEngine;
using UnityEngine.EventSystems;

public class ClickJudge : MonoBehaviour, IPointerClickHandler
{
    [Header("✅ Là đáp án đúng?")]
    public bool isCorrect = false;

    [Header("Prefab dấu V (UI)")]
    public GameObject correctMarkPrefab; // Prefab dấu ✅ (ẩn sẵn trong prefab)

    [Header("Thời gian hiển thị dấu (giây)")]
    public float markDuration = 1f;

    [Header("Canvas dùng để spawn dấu")]
    public Canvas targetCanvas;

    private Camera uiCamera;

    void Awake()
    {
        // Lấy canvas chính nếu chưa được gán
        if (targetCanvas == null)
            targetCanvas = FindFirstObjectByType<Canvas>();

        uiCamera = targetCanvas ? targetCanvas.worldCamera : Camera.main;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isCorrect) return; // ❌ chỉ phản ứng khi click đúng

        Debug.Log("✅ Click đúng!");

        SpawnMarkAtPosition(eventData.position);
    }

    private void SpawnMarkAtPosition(Vector2 screenPos)
    {
        if (correctMarkPrefab == null || targetCanvas == null)
        {
            Debug.LogWarning("⚠️ Thiếu prefab hoặc Canvas để hiển thị dấu V!");
            return;
        }

        // Tạo bản sao dấu V
        GameObject mark = Instantiate(correctMarkPrefab, targetCanvas.transform);
        RectTransform rt = mark.GetComponent<RectTransform>();

        // Đặt vị trí theo chỗ click
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetCanvas.transform as RectTransform,
            screenPos, uiCamera, out var localPos);

        rt.anchoredPosition = localPos;
        mark.SetActive(true);

        // Tự hủy sau vài giây
        Destroy(mark, markDuration);
    }
}
