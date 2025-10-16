using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Localization;               // ✅ Dùng cho LocalizedString
using UnityEngine.Localization.Settings;     // ✅ Dùng cho đổi ngôn ngữ runtime

/// <summary>
/// Hệ thống trung tâm quản lý tất cả vật phẩm có thể thu thập trong game.
/// Tự động đếm, ẩn khi thu thập, cập nhật UI, và kích hoạt sự kiện thắng khi đủ.
/// </summary>
public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }

    [Header("Tag của các vật phẩm cần đếm")]
    public string targetTag = "Collectible"; // ✅ Tag dùng chung cho mọi vật phẩm

    [Header("Hành vi khi thu thập")]
    public bool hideOnCollected = true; // Ẩn object khi được thu thập

    [Header("Sự kiện")]
    public UnityEvent onCollected;     // Gọi khi 1 vật phẩm được thu thập
    public UnityEvent onAllCollected;  // Gọi khi đã thu thập đủ toàn bộ

    [Header("UI hiển thị tiến độ")]
    public TextMeshProUGUI progressTextTMP; // Text hiển thị tiến độ
    public bool showFoundOnlyText = true;   // Chỉ hiển thị số vật phẩm đã thu

    [Header("Localization")]
    public LocalizedString foundOnlyTextLocalized; // 🔹 Key cho localized text

    [Header("Format fallback (nếu chưa có Localization)")]
    public string fallbackFoundText = "Số lượng vật phẩm đã được thu thập: {0}";
    public string fallbackProgressText = "{0}/{1} ({2:0}% )";

    private int totalCount = 0;
    private HashSet<GameObject> collectedSet = new HashSet<GameObject>();

    //=============================
    // 🔹 LIFECYCLE
    //=============================
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        RefreshTotalCount();
        UpdateUIText();

        Debug.Log($"📦 Tổng số vật phẩm cần thu thập: {totalCount}");

        // 🔹 Theo dõi khi đổi ngôn ngữ runtime
        LocalizationSettings.SelectedLocaleChanged += (locale) => UpdateUIText();
    }

    //=============================
    // 🔹 ĐẾM LẠI TỔNG VẬT PHẨM
    //=============================
    public void RefreshTotalCount()
    {
        if (string.IsNullOrEmpty(targetTag))
        {
            totalCount = 0;
            return;
        }

        try
        {
            totalCount = 0;
            var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (var obj in allObjects)
            {
                // ✅ Tính cả object ẩn, nhưng chỉ trong Scene (không tính prefab)
                if (obj.CompareTag(targetTag) && obj.scene.IsValid())
                    totalCount++;
            }
        }
        catch
        {
            totalCount = 0;
        }

        UpdateUIText();
    }

    //=============================
    // 🔹 KHI VẬT PHẨM ĐƯỢC THU THẬP
    //=============================
    public void RegisterCollected(GameObject obj)
    {
        if (obj == null) return;
        if (!obj.CompareTag(targetTag)) return;
        if (collectedSet.Contains(obj)) return;

        collectedSet.Add(obj);

        if (hideOnCollected)
            obj.SetActive(false);

        onCollected?.Invoke();
        UpdateUIText();

        Debug.Log($"✅ Đã thu thập: {collectedSet.Count}/{totalCount} ({obj.name})");

        if (collectedSet.Count >= totalCount && totalCount > 0)
        {
            Debug.Log("🎉 Đã thu thập toàn bộ vật phẩm!");
            onAllCollected?.Invoke();
        }
    }

    //=============================
    // 🔹 GETTERS
    //=============================
    public int GetTotalCount() => totalCount;
    public int GetCollectedCount() => collectedSet.Count;
    public int GetRemainingCount() => Mathf.Max(0, totalCount - collectedSet.Count);

    //=============================
    // 🔹 RESET
    //=============================
    public void ResetCollected()
    {
        collectedSet.Clear();
        UpdateUIText();
    }

    //=============================
    // 🔹 CẬP NHẬT UI
    //=============================
    void UpdateUIText()
    {
        if (progressTextTMP == null) return;

        int collected = GetCollectedCount();
        int total = GetTotalCount();
        float percent = total > 0 ? (float)collected / total * 100f : 0f;

        if (showFoundOnlyText)
        {
            if (foundOnlyTextLocalized != null && !string.IsNullOrEmpty(foundOnlyTextLocalized.TableReference))
            {
                string localizedValue = foundOnlyTextLocalized.GetLocalizedString();
                progressTextTMP.text = $"{localizedValue}: {collected}";
            }
            else
            {
                progressTextTMP.text = string.Format(fallbackFoundText, collected);
            }
        }
        else
        {
            progressTextTMP.text = string.Format(fallbackProgressText, collected, total, percent);
        }
    }

    public void SetShowFoundOnlyText(bool show)
    {
        showFoundOnlyText = show;
        UpdateUIText();
    }
}
