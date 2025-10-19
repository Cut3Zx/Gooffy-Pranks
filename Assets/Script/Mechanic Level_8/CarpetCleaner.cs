using UnityEngine;
using UnityEngine.EventSystems;

public class BroomCleaner : BaseObjectManager
{
    [Header("References")]
    public GameObject trash; // Rác cần dọn (UI hoặc object trong scene)
    private bool isCleaned = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        HandleDragStart();
        transform.SetAsLastSibling(); // đưa chổi lên trên cùng (UI)
    }

    public override void OnDrag(PointerEventData eventData)
    {
        HandleDragging(eventData);

        if (isCleaned || trash == null) return;

        // 🔹 Kiểm tra xem chổi có chạm vào rác không (nếu rác là UI)
        RectTransform trashRect = trash.GetComponent<RectTransform>();
        if (trashRect != null)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(trashRect, Input.mousePosition, canvas.worldCamera))
            {
                CleanTrash();
            }
        }
        else
        {
            // 🔹 Nếu rác là object có collider (2D/3D)
            Vector2 broomPos = transform.position;
            if (Vector2.Distance(broomPos, trash.transform.position) < 0.5f)
            {
                CleanTrash();
            }
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        HandleDragEnd();

        // ✅ Sau khi dọn xong → quay lại vị trí cũ
        ResetPosition();
    }

    private void CleanTrash()
    {
        isCleaned = true;
        Debug.Log("🧹 Chổi đã dọn sạch rác!");

        if (trash != null)
            Destroy(trash);

        // Báo cho CleanupManager
        if (CleanupManager.Instance != null)
            CleanupManager.Instance.AddFixedObject();
    }

    public override void OnPointerClick(PointerEventData eventData) { }
}
