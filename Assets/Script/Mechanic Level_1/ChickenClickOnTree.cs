using UnityEngine;
using UnityEngine.EventSystems;

public class ChickenClickOnTree : MonoBehaviour, IPointerClickHandler
{
    public GameObject completeEffect;
    public LadderDrag ladder; // 👈 gán trong Inspector (thang có biến isPlaced)
    private bool canClick = false;

    public void SetCanClick(bool state)
    {
        canClick = state;
        Debug.Log($"🐔 Gà trên cây {(state ? "có thể click" : "bị khóa")}.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ⚠️ Nếu chưa được bật hoặc thang chưa đặt thì không làm gì
        if (!canClick || (ladder != null && !ladder.isPlaced))
        {
            Debug.Log("🚫 Thang chưa tới, chưa thể bắt gà!");
            return;
        }

        Debug.Log("🐣 Bắt được con gà!");

        gameObject.SetActive(false);

        if (completeEffect != null)
            completeEffect.SetActive(true);

        if (CountingChick.Instance != null)
            CountingChick.Instance.RegisterFound(gameObject);
    }
}
