using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ElephantClick : MonoBehaviour, IPointerClickHandler
{
    [Header("Text hiển thị số thứ tự khi click")]
    public TextMeshProUGUI orderText;

    [Header("Khung viền hoặc nền highlight khi click")]
    public GameObject highlightFrame; // Kéo GameObject khung (ví dụ ảnh khung vàng) vào đây

    [HideInInspector] public int clickOrder = 0;
    private bool clicked = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clicked || ElephantOrderManager.Instance == null) return;

        clicked = true;

        // Gọi lên Manager để lấy số thứ tự chính xác
        ElephantOrderManager.Instance.OnElephantClicked(this);
    }

    /// <summary>
    /// Hiển thị số thứ tự và bật khung highlight
    /// </summary>
    public void ShowNumber(int number)
    {
        clickOrder = number;

        if (orderText != null)
        {
            orderText.text = number.ToString();
            orderText.gameObject.SetActive(true);
        }

        if (highlightFrame != null)
        {
            highlightFrame.SetActive(true);
        }
    }

    /// <summary>
    /// Reset lại trạng thái click, ẩn số và khung
    /// </summary>
    public void ResetNumber()
    {
        clicked = false;
        clickOrder = 0;

        if (orderText != null)
        {
            orderText.text = "";
            orderText.gameObject.SetActive(false);
        }

        if (highlightFrame != null)
        {
            highlightFrame.SetActive(false);
        }
    }
}
