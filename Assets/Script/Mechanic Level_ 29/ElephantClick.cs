using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ElephantClick : MonoBehaviour, IPointerClickHandler
{
    [Header("Text hiển thị số thứ tự khi click")]
    public TextMeshProUGUI orderText;

    [HideInInspector] public int clickOrder = 0;
    private bool clicked = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clicked || ElephantOrderManager.Instance == null) return;

        clicked = true;
        clickOrder = ElephantOrderManager.Instance != null
            ? ElephantOrderManager.Instance.transform.childCount // placeholder
            : 0;

        // Gọi lên Manager
        ElephantOrderManager.Instance.OnElephantClicked(this);
    }

    public void ShowNumber(int number)
    {
        clickOrder = number;

        if (orderText != null)
        {
            orderText.text = number.ToString();
            orderText.gameObject.SetActive(true);
        }
    }

    public void ResetNumber()
    {
        clicked = false;
        clickOrder = 0;

        if (orderText != null)
        {
            orderText.text = "";
            orderText.gameObject.SetActive(false);
        }
    }
}
