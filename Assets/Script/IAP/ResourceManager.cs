using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [Header("Tài nguyên hiện tại")]
    public int coins = 0;
    public int hints = 0;

    [Header("UI Text để hiển thị")]
    // MỚI THAY ĐỔI: Chuyển từ biến đơn lẻ sang List (danh sách)
    public List<TextMeshProUGUI> coinTexts;   // Kéo TẤT CẢ Text coin vào đây
    public List<TextMeshProUGUI> hintTexts;   // Kéo TẤT CẢ Text hint vào đây

    [Header("Events (tuỳ chọn)")]
    public UnityEvent<int> OnCoinsChanged;
    public UnityEvent<int> OnHintsChanged;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadResources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    // ========== THÊM TÀI NGUYÊN ==========
    public void AddCoins(int amount)
    {
        coins += amount;
        SaveResources();
        UpdateUI();
        OnCoinsChanged?.Invoke(coins);

        Debug.Log($"💰 +{amount} Coins! Tổng: {coins}");
    }

    public void AddHints(int amount)
    {
        hints += amount;
        SaveResources();
        UpdateUI();
        OnHintsChanged?.Invoke(hints);

        Debug.Log($"💡 +{amount} Hints! Tổng: {hints}");
    }

    // ========== TRỪ TÀI NGUYÊN ==========
    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            SaveResources();
            UpdateUI();
            OnCoinsChanged?.Invoke(coins);

            Debug.Log($"💰 -{amount} Coins! Còn: {coins}");
            return true;
        }
        else
        {
            Debug.Log("❌ Không đủ coins!");
            return false;
        }
    }

    public bool SpendHints(int amount)
    {
        if (hints >= amount)
        {
            hints -= amount;
            SaveResources();
            UpdateUI();
            OnHintsChanged?.Invoke(hints);

            Debug.Log($"💡 -{amount} Hints! Còn: {hints}");
            return true;
        }
        else
        {
            Debug.Log("❌ Không đủ hints!");
            return false;
        }
    }

    // ========== KIỂM TRA ==========
    public bool HasEnoughCoins(int amount)
    {
        return coins >= amount;
    }

    public bool HasEnoughHints(int amount)
    {
        return hints >= amount;
    }

    // ========== LƯU & TẢI DỮ LIỆU ==========
    void SaveResources()
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("Hints", hints);
        PlayerPrefs.Save();
    }

    void LoadResources()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        hints = PlayerPrefs.GetInt("Hints", 0);
    }

    // ========== CẬP NHẬT UI ==========
    public void UpdateUI()
    {
        // MỚI THAY ĐỔI: Dùng vòng lặp để cập nhật TẤT CẢ các text trong danh sách
        foreach (TextMeshProUGUI text in coinTexts)
        {
            if (text != null)
                text.text = coins.ToString();
        }

        foreach (TextMeshProUGUI text in hintTexts)
        {
            if (text != null)
                text.text = hints.ToString();
        }
    }

    // ========== RESET (TEST) ==========
    public void ResetResources()
    {
        coins = 0;
        hints = 0;
        SaveResources();
        UpdateUI();
        Debug.Log("🔄 Đã reset tài nguyên!");
    }

    // ========== CHEAT (TEST) ==========
    [ContextMenu("Add 100 Coins")]
    void CheatCoins()
    {
        AddCoins(100);
    }

    [ContextMenu("Add 10 Hints")]
    void CheatHints()
    {
        AddHints(10);
    }
}