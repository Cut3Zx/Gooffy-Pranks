using UnityEngine;
using System.Collections;

public class CleanupManager : MonoBehaviour
{
    public static CleanupManager Instance { get; private set; }

    private int fixedCount = 0;
    public int totalObjects = 3; // Tủ + Tranh + Thảm

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddFixedObject()
    {
        fixedCount++;
        Debug.Log($"🧩 Đã sửa {fixedCount}/{totalObjects} vật.");

        if (fixedCount >= totalObjects)
        {
            Debug.Log("🎉 Phòng khách đã dọn xong!");
            StartCoroutine(DelayWin(1f)); // ⏳ delay 3 giây
        }
    }

    private IEnumerator DelayWin(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (GameManager.Instance != null)
            GameManager.Instance.EndGame(true);
        else
            Debug.LogWarning("⚠️ Không tìm thấy GameManager!");
    }

    public void ResetProgress()
    {
        fixedCount = 0;
    }
}
