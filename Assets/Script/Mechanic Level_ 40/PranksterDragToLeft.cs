using UnityEngine;
using UnityEngine.EventSystems;

public class PranksterDragToLeft : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI thắng khi kéo đúng")]
    public GameObject winUI;

    [Header("Giới hạn kéo theo trục X (UI)")]
    public float minX = -300f; // kéo trái đến đây là thắng
    public float maxX = 300f;  // không cho kéo sang phải quá xa

    [Header("Tốc độ theo tay (UI Canvas)")]
    public float dragSpeed = 1f;

    private RectTransform rect;
    private Canvas canvas;
    private Vector2 startPos;
    private bool isWin = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        startPos = rect.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        if (isWin) return;
    }

    public void OnDrag(PointerEventData e)
    {
        if (isWin) return;

        // chỉ cho kéo ngang
        float deltaX = e.delta.x / canvas.scaleFactor * dragSpeed;
        rect.anchoredPosition += new Vector2(deltaX, 0);

        // giới hạn phạm vi kéo
        float clampedX = Mathf.Clamp(rect.anchoredPosition.x, minX, maxX);
        rect.anchoredPosition = new Vector2(clampedX, startPos.y);
    }

    public void OnEndDrag(PointerEventData e)
    {
        if (isWin) return;

        // kiểm tra nếu kéo đủ xa sang trái → thắng
        if (rect.anchoredPosition.x <= minX + 10f)
        {
            Debug.Log("✅ Kéo sang trái — WIN!");
            isWin = true;

            // 🏁 Gọi GameManager
            if (GameManager.Instance != null)
                GameManager.Instance.EndGame(true);

            // 🔓 Mở khóa level tiếp theo
            

            // 🎉 Hiện UI thắng
            if (winUI != null)
                winUI.SetActive(true);
        }
        else
        {
            // Nếu không kéo đủ xa thì quay lại vị trí cũ
            rect.anchoredPosition = startPos;
        }
    }
}
