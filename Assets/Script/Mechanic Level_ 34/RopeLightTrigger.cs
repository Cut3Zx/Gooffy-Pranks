using UnityEngine;
using UnityEngine.EventSystems;

public class RopeLightTrigger : MonoBehaviour, IPointerDownHandler
{
    public RectTransform rope;      // dây thừng
    public GameObject lightObject;  // object đèn (ẩn sẵn)
    public float pullLength = 100f; // độ dài kéo xuống
    public float pullSpeed = 5f;    // tốc độ kéo & quay lại

    private Vector2 startPos;
    private bool isPulled = false;

    void Start() => startPos = rope.anchoredPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPulled) return;
        StartCoroutine(PullAndReturn());
    }

    private System.Collections.IEnumerator PullAndReturn()
    {
        isPulled = true;
        Vector2 targetPos = startPos - new Vector2(0, pullLength);

        // Kéo xuống
        while (Vector2.Distance(rope.anchoredPosition, targetPos) > 0.1f)
        {
            rope.anchoredPosition = Vector2.Lerp(rope.anchoredPosition, targetPos, Time.deltaTime * pullSpeed);
            yield return null;
        }

        // Quay lại
        while (Vector2.Distance(rope.anchoredPosition, startPos) > 0.1f)
        {
            rope.anchoredPosition = Vector2.Lerp(rope.anchoredPosition, startPos, Time.deltaTime * pullSpeed);
            yield return null;
        }

        // Bật đèn
        lightObject.SetActive(true);
        isPulled = false;
    }
}
