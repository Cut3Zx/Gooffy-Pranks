using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Localization;                // ✅ Dùng cho LocalizedString
using UnityEngine.Localization.Settings;      // ✅ Dùng cho đổi ngôn ngữ runtime

public class CountingChick : MonoBehaviour
{
    public static CountingChick Instance { get; private set; }

    [Header("Tag của các object cần đếm")]
    public string targetTag = "KFC";

    [Header("Hành vi khi đánh dấu đã tìm thấy")]
    public bool hideOnFound = true; // ẩn object khi tìm thấy

    [Header("Sự kiện")]
    public UnityEvent onFound;     // gọi khi một object được tìm thấy
    public UnityEvent onAllFound;  // gọi khi đã tìm thấy tất cả

    [Header("UI hiển thị tiến độ")]
    public TextMeshProUGUI progressTextTMP; // hiển thị text
    public bool showFoundOnlyText = true;   // chỉ hiển thị số đã tìm

    [Header("Localization")]
    public LocalizedString foundOnlyTextLocalized; // 🔹 Kết nối tới key trong bảng (VD: GameTextTable/hintlevel1)

    [Header("Format fallback (nếu chưa có Localization)")]
    public string fallbackFoundText = "Số lượng gà con đã được tìm thấy là: {0}";
    public string fallbackProgressText = "{0}/{1} ({2:0}% )";

    private int totalCount = 0;
    private HashSet<GameObject> foundSet = new HashSet<GameObject>();

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

        Debug.Log($"🐥 Tổng số gà đếm được: {totalCount}");

        // 🔹 Theo dõi khi người chơi đổi ngôn ngữ runtime
        LocalizationSettings.SelectedLocaleChanged += (locale) => UpdateUIText();
    }

    // Đếm lại tổng số object có tag
    public void RefreshTotalCount()
    {
        if (string.IsNullOrEmpty(targetTag))
        {
            totalCount = 0;
            return;
        }

        try
        {
            var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            totalCount = 0;
            foreach (var obj in allObjects)
            {
                if (obj.CompareTag(targetTag))
                    totalCount++;
            }
        }
        catch
        {
            totalCount = 0;
        }

        UpdateUIText();
    }

    // Gọi khi một object bị bấm/thu thập
    public void RegisterFound(GameObject obj)
    {
        if (obj == null) return;
        if (!obj.CompareTag(targetTag)) return;
        if (foundSet.Contains(obj)) return;

        foundSet.Add(obj);
        if (hideOnFound)
            obj.SetActive(false);

        onFound?.Invoke();
        UpdateUIText();

        if (foundSet.Count >= totalCount && totalCount > 0)
        {
            Debug.Log("🎉 Đã tìm thấy toàn bộ gà!");
            onAllFound?.Invoke();
        }
    }

    public int GetTotalCount() => totalCount;
    public int GetFoundCount() => foundSet.Count;
    public int GetRemainingCount() => Mathf.Max(0, totalCount - foundSet.Count);

    public void ResetFound()
    {
        foundSet.Clear();
        UpdateUIText();
    }

    void UpdateUIText()
    {
        if (progressTextTMP == null) return;

        int found = GetFoundCount();
        int total = GetTotalCount();
        float percent = total > 0 ? (float)found / total * 100f : 0f;

        if (showFoundOnlyText)
        {
            if (foundOnlyTextLocalized != null && !string.IsNullOrEmpty(foundOnlyTextLocalized.TableReference))
            {
                // ✅ Cách an toàn, tương thích với Unity 2022 trở lên
                string localizedValue = foundOnlyTextLocalized.GetLocalizedString();
                progressTextTMP.text = $"{localizedValue}: {found}";
            }
            else
            {
                progressTextTMP.text = string.Format(fallbackFoundText, found);
            }
        }
        else
        {
            progressTextTMP.text = string.Format(fallbackProgressText, found, total, percent);
        }
    }


    public void SetShowFoundOnlyText(bool show)
    {
        showFoundOnlyText = show;
        UpdateUIText();
    }
}
