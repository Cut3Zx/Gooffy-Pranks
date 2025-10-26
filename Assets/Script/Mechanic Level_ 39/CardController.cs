using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerClickHandler
{
    [Header("Ảnh mặt trước của thẻ")]
    public GameObject front;

    [Header("Ảnh mặt sau của thẻ")]
    public GameObject back;

    [Header("Tên định danh để so sánh (ví dụ: 'Cat', 'Dog', 'Apple')")]
    public string cardID;

    private bool isFlipped = false;
    private bool isMatched = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isMatched || isFlipped) return; // đã khớp hoặc đang mở thì bỏ qua

        Flip(true);
        FindPairManager.Instance.CheckCard(this);
    }

    public void Flip(bool showFront)
    {
        isFlipped = showFront;
        front.SetActive(showFront);
        back.SetActive(!showFront);
    }

    public void SetMatched()
    {
        isMatched = true;
        // Ẩn thẻ sau khi khớp
        StartCoroutine(HideAfterDelay(0.3f));
    }

    private System.Collections.IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
