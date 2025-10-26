using UnityEngine;
using UnityEngine.EventSystems;

public class BurgerMerge : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Prefab burger to sau khi ghép đủ")]
    public GameObject mergedBurgerPrefab; // 🍔 Burger to (đặt prefab ở đây)

    [Header("Khoảng cách để hợp lại")]
    public float mergeDistance = 150f;

    [Header("Số lượng burger nhỏ cần ghép")]
    public int burgersNeeded = 2;

    private static int mergedCount = 0;
    private RectTransform rect;
    private Canvas canvas;
    private bool merged = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData e) { }

    public void OnDrag(PointerEventData e)
    {
        if (merged) return;
        rect.anchoredPosition += e.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData e)
    {
        if (merged) return;

        // Kiểm tra va chạm với burger khác
        BurgerMerge[] all = FindObjectsOfType<BurgerMerge>();
        foreach (var other in all)
        {
            if (other == this || other.merged) continue;

            float dist = Vector2.Distance(rect.anchoredPosition, other.rect.anchoredPosition);
            if (dist < mergeDistance)
            {
                MergeWith(other);
                break;
            }
        }
    }

    private void MergeWith(BurgerMerge other)
    {
        if (mergedBurgerPrefab == null) return;

        merged = true;
        other.merged = true;
        mergedCount++;

        Vector2 middle = (rect.anchoredPosition + other.rect.anchoredPosition) / 2f;

        // Ẩn 2 cái cũ
        gameObject.SetActive(false);
        other.gameObject.SetActive(false);

        // Nếu đã ghép đủ burger
        if (mergedCount >= burgersNeeded - 1)
        {
            GameObject newBurger = Instantiate(mergedBurgerPrefab, rect.parent);
            newBurger.GetComponent<RectTransform>().anchoredPosition = middle;
            Debug.Log("🍔 Đã tạo burger to!");
        }
        else
        {
            Debug.Log("🥯 Hai burger nhỏ đã hợp lại (" + mergedCount + ")");
        }
    }
}
