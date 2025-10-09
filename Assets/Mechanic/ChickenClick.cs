using UnityEngine;
using UnityEngine.EventSystems;

public class ChickenClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject completeEffect; // hiệu ứng hoặc thông báo hoàn thành (tùy chọn)

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("🐣 Bắt được con gà!");

        // Ẩn gà khi được click
        gameObject.SetActive(false);

        // Nếu có hiệu ứng hoặc popup hoàn thành, bật nó
        if (completeEffect != null)
            completeEffect.SetActive(true);
    }
}
