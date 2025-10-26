using UnityEngine;
using System.Collections;

[DefaultExecutionOrder(-5)]
public class WeighSnapManager : MonoBehaviour
{
    public static WeighSnapManager Instance { get; private set; }

    [Header("Thiết lập tổng số vật cần gắn")]
    public int totalObjects = 2;
    private int snappedCount = 0;

    [Header("Ảnh chiến thắng")]
    public GameObject winImage;

    [Header("Các object cần ẩn khi thắng")]
    public GameObject weighBase;
    public GameObject weighBar;
    public GameObject weight;
    public GameObject elephant;
    public GameObject dino;

    [Header("⏱️ Thời gian chờ trước khi hiển thị win (giây)")]
    public float winDelay = 0.8f;

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
            StartCoroutine(ShowWinWithDelay());
    }

    private IEnumerator ShowWinWithDelay()
    {
        Debug.Log("🎉 Đủ vật — chuẩn bị hiện ảnh thắng...");
        yield return new WaitForSeconds(winDelay);

        // Ẩn tất cả vật và cân
        if (weighBase) weighBase.SetActive(false);
        if (weighBar) weighBar.SetActive(false);
        if (weight) weight.SetActive(false);
        if (elephant) elephant.SetActive(false);
        if (dino) dino.SetActive(false);

        // Hiện ảnh thắng
        if (winImage)
        {
            winImage.SetActive(true);
            Debug.Log($"🏆 Ảnh thắng đã hiện sau {winDelay} giây!");
        }
    }
}
