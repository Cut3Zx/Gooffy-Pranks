using UnityEngine;
using System.Collections;

[DefaultExecutionOrder(-5)] // đảm bảo khởi tạo sớm
public class SnapManager : MonoBehaviour
{
    public static SnapManager Instance { get; private set; }

    [Header("Thiết lập")]
    public int totalObjects = 2;
    private int snappedCount = 0;

    [Header("Tham chiếu gấu")]
    public GameObject bearObject;
    public FollowAndWin bearFollowScript;

    [Header("Tùy chọn")]
    public float bearStartDelay = 1.5f; // delay 1.5s trước khi gấu bắt đầu di chuyển

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterSnappedObject()
    {
        snappedCount++;
        Debug.Log($"🧩 Đã gắn {snappedCount}/{totalObjects}");

        if (snappedCount >= totalObjects)
        {
            Debug.Log("🎉 Tất cả vật đã gắn xong, chuẩn bị gọi gấu!");
            StartCoroutine(ActivateBearAfterDelay());
        }
    }

    private IEnumerator ActivateBearAfterDelay()
    {
        yield return new WaitForSeconds(bearStartDelay);

        if (bearObject != null)
        {
            bearObject.SetActive(true);
            Debug.Log("🐻 Gấu đã được bật!");
        }

        if (bearFollowScript != null)
        {
            bearFollowScript.enabled = true;
            Debug.Log("🐾 Gấu bắt đầu đuổi target!");
        }
        else
        {
            Debug.LogWarning("⚠️ Chưa gán script FollowAndWin cho bear!");
        }
    }
}
