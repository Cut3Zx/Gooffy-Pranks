using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerClickHandler
{
    [Header("Ảnh mặt trước của thẻ")]
    public GameObject front;

    [Header("Ảnh mặt sau của thẻ")]
    public GameObject back;

    [Header("Tên định danh (ví dụ: 'Cat', 'Dog')")]
    public string cardID;

    private bool isFlipped = false;
    private bool isMatched = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isMatched || isFlipped) return;

        // 🚫 Khi FindPairManager đang check, không cho click
        if (FindPairManager.Instance != null && FindPairManager.Instance.IsChecking())
            return;

        Flip(true);
        FindPairManager.Instance.CheckCard(this);
    }

    public void Flip(bool showFront)
    {
        Debug.Log($"🃏 Flip {cardID} → {(showFront ? "Front" : "Back")}");
        isFlipped = showFront;
        front.SetActive(showFront);
        back.SetActive(!showFront);
    }

    public void SetMatched()
    {
        isMatched = true;
        StartCoroutine(HideAfterDelay(0.25f));
    }

    private System.Collections.IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
    

}
