using UnityEngine;

public class FollowAndWin : BaseObjectManager
{
    [Header("Cài đặt mục tiêu")]
    public Transform target;          // Mục tiêu đuổi theo
    public float speed = 3f;          // Tốc độ di chuyển
    public float stopDistance = 0.2f; // Khoảng cách để tính là "chạm"
    public bool faceTarget = false;   // Xoay hướng theo mục tiêu

    [Header("Hành vi khi chạm")]
    public bool callWinOnTouch = true;      // Có gọi thắng khi chạm không
    public bool onlyFollowWhenActive = true;// Chỉ đuổi khi target đang bật
    public GameObject replacementObject;    // ✅ Object thay thế khi chạm

    private bool isFollowing = false;
    private bool hasTouched = false;

    private void Update()
    {
        if (hasTouched || target == null) return;

        // Nếu chọn chỉ đuổi khi target active
        if (onlyFollowWhenActive && !target.gameObject.activeInHierarchy)
        {
            isFollowing = false;
            return;
        }

        if (!isFollowing)
        {
            isFollowing = true;
            SFXManager.Instance.PlaySFX("Bee");
            SFXManager.Instance.PlaySFX("Bat");
            Debug.Log($"{gameObject.name} bắt đầu đuổi {target.name}");
        }

        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;

        // ✅ Khi chạm target
        if (distance <= stopDistance)
        {
            hasTouched = true;
            isFollowing = false;

            Debug.Log($"🎯 {gameObject.name} đã chạm {target.name}!");

            // ✅ Ẩn object hiện tại
            gameObject.SetActive(false);

            // ✅ Hiện object thay thế (nếu có)
            if (replacementObject != null)
            {
                replacementObject.SetActive(true);
                Debug.Log($"🔄 Hiện object thay thế: {replacementObject.name}");
            }

            // ✅ Gọi thắng (nếu được bật)
            if (callWinOnTouch && GameManager.Instance != null)
            {
                GameManager.Instance.EndGame(true);
                Debug.Log("🏆 Gọi thắng!");
            }

            return;
        }

        // Di chuyển mượt
        Vector3 moveDir = direction.normalized;
        Vector3 moveStep = moveDir * speed * Time.deltaTime;

        if (moveStep.magnitude > distance)
            moveStep = moveDir * distance;

        transform.position += moveStep;

        // Xoay mặt theo hướng di chuyển
        if (faceTarget && moveDir != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
