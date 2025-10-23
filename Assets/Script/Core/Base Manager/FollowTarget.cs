using UnityEngine;

public class FollowAndWin : BaseObjectManager
{
    [Header("Cài đặt mục tiêu")]
    public Transform target;          // Mục tiêu đuổi theo
    public float speed = 3f;          // Tốc độ di chuyển
    public float stopDistance = 0.2f; // Khoảng cách để tính là "chạm"
    public bool faceTarget = false;   // Xoay hướng theo mục tiêu

    [Header("Tùy chọn hành vi")]
    public bool callWinOnTouch = true;      // Có gọi thắng khi chạm không
    public bool onlyFollowWhenActive = true;// Chỉ đuổi khi target đang bật

    private bool isFollowing = false;
    private bool hasTouched = false; // ✅ Trạng thái đã chạm rồi thì dừng hoàn toàn

    private void Update()
    {
        if (hasTouched || target == null) return;

        // Nếu chọn chỉ đuổi khi active
        if (onlyFollowWhenActive && !target.gameObject.activeInHierarchy)
        {
            isFollowing = false;
            return;
        }

        // Bắt đầu đuổi nếu chưa
        if (!isFollowing)
        {
            isFollowing = true;
            Debug.Log($"{gameObject.name} bắt đầu đuổi {target.name}");
        }

        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;

        // ✅ Nếu chạm rồi
        if (distance <= stopDistance)
        {
            Debug.Log($"🎯 {gameObject.name} đã chạm {target.name}!");

            // Dừng hoàn toàn việc di chuyển
            hasTouched = true;
            isFollowing = false;

            // ✅ Gọi thắng (nếu được phép)
            if (callWinOnTouch && GameManager.Instance != null)
            {
                GameManager.Instance.EndGame(true);
                Debug.Log("🏆 Gọi thắng!");
            }

            return;
        }

        // Di chuyển mượt hơn (không vượt quá target)
        Vector3 moveDir = direction.normalized;
        Vector3 moveStep = moveDir * speed * Time.deltaTime;

        // Nếu bước đi vượt quá khoảng cách còn lại thì dừng lại ở mép
        if (moveStep.magnitude > distance)
            moveStep = moveDir * distance;

        transform.position += moveStep;

        // Xoay mặt theo hướng
        if (faceTarget && moveDir != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    // Cho phép gán target từ script khác
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        hasTouched = false;
        isFollowing = false;
    }
}
