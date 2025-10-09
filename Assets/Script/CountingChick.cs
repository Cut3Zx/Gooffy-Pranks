using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Quản lý việc đếm "gà" (các GameObject có tag chỉ định, ví dụ "KFC").
/// Gọi RegisterFound(gameObject) khi người chơi bấm vào object đó.
/// </summary>
public class CountingChick : MonoBehaviour
{
    public static CountingChick Instance { get; private set; }

    [Header("Tag của các object cần đếm")]
    public string targetTag = "KFC";

    [Header("Hành vi khi đánh dấu đã tìm thấy")]
    public bool hideOnFound = true; // ẩn object khi tìm thấy

    [Header("Events")]
    public UnityEvent onFound; // gọi khi một object được tìm thấy
    public UnityEvent onAllFound; // gọi khi đã tìm thấy tất cả

    [Header("UI Text (text-only mode)")]
    public TextMeshProUGUI progressTextTMP; // hiển thị dạng "found/total (xx%)"
    public string progressTextFormat = "{0}/{1} ({2:0}% )"; // {found},{total},{percent}
    [Header("Found-only text option")]
    // Mặc định hiển thị chỉ số đã tìm (không hiện 0/0)
    public bool showFoundOnlyText = true; // nếu true sẽ hiển thị chỉ số đã tìm
    public string foundOnlyTextFormat = "Đã tìm thấy: {0}"; // {found}

    int totalCount = 0;
    HashSet<GameObject> foundSet = new HashSet<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        RefreshTotalCount();
        UpdateUIText();
    }

    // Đếm lại tổng số object có tag (gọi khi cần, ví dụ khi spawn động)
    public void RefreshTotalCount()
    {
        if (string.IsNullOrEmpty(targetTag))
        {
            totalCount = 0;
            return;
        }

        try
        {
            var arr = GameObject.FindGameObjectsWithTag(targetTag);
            totalCount = arr != null ? arr.Length : 0;
        }
        catch
        {
            // nếu tag không tồn tại hoặc lỗi, đặt 0
            totalCount = 0;
        }
        UpdateUIText();
    }

    // Gọi khi một object bị bấm/thu thập. Có thể gọi trực tiếp từ object đó.
    public void RegisterFound(GameObject obj)
    {
        if (obj == null) return;

        // Chỉ xử lý nếu object thực sự có tag tương ứng (phòng trường hợp vô tình gọi nhầm)
        if (!string.IsNullOrEmpty(targetTag) && !obj.CompareTag(targetTag))
            return;

        if (foundSet.Contains(obj))
            return; // đã đánh dấu trước đó

        foundSet.Add(obj);

        if (hideOnFound)
        {
            obj.SetActive(false);
        }

        // Gọi sự kiện
        onFound?.Invoke();

        UpdateUIText();

        // Nếu đã tìm hết
        if (foundSet.Count >= totalCount && totalCount > 0)
        {
            onAllFound?.Invoke();
        }
    }

    // Hàm tiện: có thể gán cho Button.OnClick và truyền chính GameObject đó
    public void OnClickMarkFound(GameObject obj)
    {
        RegisterFound(obj);
    }

    // Trả về tổng số 'gà' trên map (theo tag) - lưu ý: nếu spawn động, hãy gọi RefreshTotalCount trước
    public int GetTotalCount()
    {
        return totalCount;
    }

    public int GetFoundCount()
    {
        return foundSet.Count;
    }

    public int GetRemainingCount()
    {
        return Mathf.Max(0, totalCount - foundSet.Count);
    }

    // Reset (clear) danh sách đã tìm thấy - không ảnh hưởng đến các object
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
        float percent = total > 0 ? (float)found / (float)total * 100f : 0f;
        string txt;
        if (showFoundOnlyText)
        {
            txt = string.Format(foundOnlyTextFormat, found);
        }
        else
        {
            txt = string.Format(progressTextFormat, found, total, percent);
        }

        if (progressTextTMP != null)
            progressTextTMP.text = txt;
    }

    // Thay đổi chế độ hiển thị
    public void SetShowFoundOnlyText(bool show)
    {
        showFoundOnlyText = show;
        UpdateUIText();
    }
}
