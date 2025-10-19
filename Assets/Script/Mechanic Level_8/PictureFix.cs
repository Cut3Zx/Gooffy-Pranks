using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PictureMountSnap : BaseObjectManager
{
    [Header("References")]
    public GameObject mountedPicture;   // Hình tranh treo trên tường (ẩn ban đầu)
    public Image groundPicture;         // Tranh đang nằm dưới đất
    public RectTransform snapPoint;     // Vị trí “treo tranh” (trên tường)

    private bool isSnapped = false;     // Đã treo hay chưa

    protected override void Awake()
    {
        base.Awake();

        if (mountedPicture != null)
            mountedPicture.SetActive(false);

        if (groundPicture == null)
            groundPicture = GetComponent<Image>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        HandleDragStart();

        // Ẩn tranh dưới đất, hiện bản treo di chuyển theo tay
        if (groundPicture != null)
            groundPicture.enabled = false;

        if (mountedPicture != null)
        {
            mountedPicture.SetActive(true);
            mountedPicture.transform.position = transform.position;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        // Di chuyển bản treo theo ngón tay
        if (mountedPicture != null)
            mountedPicture.transform.position += (Vector3)eventData.delta;

        // Kiểm tra nếu chạm vùng treo
        if (snapPoint != null && RectTransformUtility.RectangleContainsScreenPoint(
                snapPoint, mountedPicture.transform.position, canvas.worldCamera))
        {
            isSnapped = true;
            // Ghim tranh vào vị trí snapPoint
            mountedPicture.transform.position = snapPoint.position;
        }
        else
        {
            isSnapped = false;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        HandleDragEnd();

        if (isSnapped)
        {
            // Treo tranh thành công => giữ nguyên bản treo, ẩn tranh rơi
            Debug.Log("🖼 Tranh đã được treo lên tường!");
            groundPicture.enabled = false;
            mountedPicture.SetActive(true);
            if (CleanupManager.Instance != null)
            {
                CleanupManager.Instance.AddFixedObject();
            }
        }
        else
        {
            // Thả sai => trở về vị trí cũ
            mountedPicture.SetActive(false);
            groundPicture.enabled = true;
            ResetPosition();
        }
    }

    public override void OnPointerClick(PointerEventData eventData) { }
}
