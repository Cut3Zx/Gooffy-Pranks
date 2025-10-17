using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BroomClick : BaseObjectManager
{
    [Header("Liên kết các object tiếp theo")]
    public GameObject cat; // 🐈 Con mèo sẽ dậy khi chổi ngã
    public Transform pivotPoint; // 🔸 Điểm xoay ở cuối cán chổi (pivot)

    [Header("Hiệu ứng ngã")]
    public float fallAngle = 45f;       // Góc nghiêng tối đa
    public float fallDuration = 1f;     // Thời gian ngã

    private bool hasFallen = false;
    private float currentAngle = 0f;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (hasFallen)
        {
            Debug.Log("⚠️ Chổi đã ngã rồi.");
            return;
        }

        HandleClick();
        Debug.Log("🧹 Click vào chổi — bắt đầu ngã!");
        StartCoroutine(FallOver());
    }

    private IEnumerator FallOver()
    {
        hasFallen = true;
        float elapsed = 0f;

        Vector3 pivot = pivotPoint != null ? pivotPoint.position : transform.position;

        while (elapsed < fallDuration)
        {
            float t = elapsed / fallDuration;
            float targetAngle = Mathf.Lerp(0f, -fallAngle, t);

            // Xoay quanh pivot (từ góc hiện tại đến góc mới)
            float deltaAngle = targetAngle - currentAngle;
            transform.RotateAround(pivot, Vector3.forward, deltaAngle);
            currentAngle = targetAngle;

            elapsed += Time.deltaTime;
            yield return null;
        }

        Debug.Log("💥 Chổi ngã xong — gọi mèo dậy!");

        // Gọi mèo tỉnh dậy
        if (cat != null)
        {
            CatBehavior cb = cat.GetComponent<CatBehavior>();
            if (cb != null)
            {
                Debug.Log("🐈 Gọi CatBehavior.WakeUp()!");
                cb.WakeUp();
            }
            else
            {
                Debug.LogWarning("⚠️ Không tìm thấy CatBehavior trên object mèo!");
            }
        }
    }

    // ❌ Không cho kéo
    public override void OnBeginDrag(PointerEventData e) { }
    public override void OnDrag(PointerEventData e) { }
    public override void OnEndDrag(PointerEventData e) { }
}
