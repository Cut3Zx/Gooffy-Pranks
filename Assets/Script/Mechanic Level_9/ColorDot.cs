using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorDot : BaseObjectManager
{
    [Header("Tên màu của chấm (red, yellow, blue)")]
    public string colorName;

    [Header("Prefab màu pha (cam, xanh lá, tím)")]
    public GameObject orangePrefab;
    public GameObject greenPrefab;
    public GameObject purplePrefab;

    private bool isDragging = false;
    private ColorDot collidedDot = null;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        HandleDragStart();
        transform.SetAsLastSibling();
        isDragging = true;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        HandleDragging(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        HandleDragEnd();
        isDragging = false;

        if (collidedDot != null)
        {
            TryMixColor(collidedDot);
        }

        ResetPosition();
    }

    private void TryMixColor(ColorDot other)
    {
        string combo = $"{colorName}-{other.colorName}";
        Debug.Log($"🎨 Kết hợp: {combo}");

        GameObject prefabToSpawn = null;
        bool isWin = false;

        // ✅ Đỏ + Vàng = Cam
        if ((colorName == "red" && other.colorName == "yellow") ||
            (colorName == "yellow" && other.colorName == "red"))
        {
            prefabToSpawn = orangePrefab;
            isWin = true;
        }
        // ❌ Vàng + Xanh = Xanh lá
        else if ((colorName == "yellow" && other.colorName == "blue") ||
                 (colorName == "blue" && other.colorName == "yellow"))
        {
            prefabToSpawn = greenPrefab;
        }
        // ❌ Đỏ + Xanh = Tím
        else if ((colorName == "red" && other.colorName == "blue") ||
                 (colorName == "blue" && other.colorName == "red"))
        {
            prefabToSpawn = purplePrefab;
        }

        // Nếu không có màu phù hợp -> return
        if (prefabToSpawn == null)
            return;

        // 🔸 Ẩn cả 2 chấm
        gameObject.SetActive(false);
        other.gameObject.SetActive(false);

        // 🔸 Tính vị trí trung điểm giữa 2 chấm
        Vector3 midPos = (transform.position + other.transform.position) / 2f;

        // 🔸 Sinh ra màu pha tại giữa
        GameObject newColor = Instantiate(prefabToSpawn, transform.parent);
        newColor.transform.position = midPos;
        newColor.SetActive(true);

        Debug.Log($"🟠 Tạo ra {prefabToSpawn.name} tại {midPos}");

        // ✅ Nếu đúng màu (cam) thì thắng, ngược lại thua
        if (isWin)
        {
            Debug.Log("🎉 Tạo thành màu cam → THẮNG!");
            if (GameManager.Instance != null)
                GameManager.Instance.StartCoroutine(DelayEndGame(true, 1.5f)); // 👈 chạy từ GameManager
        }
        else
        {
            Debug.Log("❌ Sai tổ hợp → THUA!");
            if (GameManager.Instance != null)
                GameManager.Instance.StartCoroutine(DelayEndGame(false, 1.5f));
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var dot = other.GetComponent<ColorDot>();
        if (dot != null && dot != this)
        {
            collidedDot = dot;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var dot = other.GetComponent<ColorDot>();
        if (dot != null && dot == collidedDot)
        {
            collidedDot = null;
        }
    }
    private System.Collections.IEnumerator DelayEndGame(bool isWin, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.EndGame(isWin);
    }

}
