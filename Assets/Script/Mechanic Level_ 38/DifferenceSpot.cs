using UnityEngine;
using UnityEngine.EventSystems;

public class DifferenceSpotPair : MonoBehaviour, IPointerClickHandler
{
    [Header("Vòng tròn xanh hiển thị khi tìm ra")]
    public GameObject highlightCircle; // ẩn sẵn

    [Header("Điểm tương ứng ở ảnh còn lại")]
    public DifferenceSpotPair pairedSpot; // kéo object bên kia vào

    private bool found = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (found) return;
        found = true;

        // Hiện vòng tròn xanh tại chỗ
        if (highlightCircle != null)
            highlightCircle.SetActive(true);

        // Hiện vòng tròn bên cặp còn lại
        if (pairedSpot != null && pairedSpot.highlightCircle != null)
        {
            pairedSpot.highlightCircle.SetActive(true);
            pairedSpot.found = true; // đánh dấu cũng đã tìm
        }

        Debug.Log($"✅ Phát hiện điểm khác biệt: {name}");

        // Báo cho Manager (nếu có)
        FindDifferenceManager.Instance?.RegisterFound();
    }
}
