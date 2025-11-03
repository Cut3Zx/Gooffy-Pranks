using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPCallbacks : MonoBehaviour
{
    public static IAPCallbacks Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ========== CALLBACK: KHỞI TẠO ==========
    public void OnInitializeSuccess()
    {
        Debug.Log("[Callback] IAP đã sẵn sàng");
        // Cập nhật UI shop nếu cần
    }

    public void OnInitializeFailed(string error)
    {
        Debug.LogError($"[Callback] IAP lỗi: {error}");
        // Hiển thị thông báo lỗi
    }

    // ========== CALLBACK: MUA THÀNH CÔNG ==========
    public void OnPurchaseSuccess(string productId)
    {
        Debug.Log($"[Callback] Mua thành công: {productId}");

        // Lấy thông tin sản phẩm
        var info = IAPProductConfig.GetProductInfo(productId);

        // Thêm tài nguyên
        if (ResourceManager.Instance != null)
        {
            if (info.coinAmount > 0)
                ResourceManager.Instance.AddCoins(info.coinAmount);

            if (info.hintAmount > 0)
                ResourceManager.Instance.AddHints(info.hintAmount);
        }

        // Xử lý non-consumable
        if (productId == IAPProductConfig.ADS_BLOCK)
        {
            UnlockAdsBlock();
        }
        else if (productId == IAPProductConfig.SUPER_PACK)
        {
            UnlockSuperPack();
        }
        else if (productId == IAPProductConfig.STARTER_PACK)
        {
            UnlockStarterPack();
        }

        // Hiển thị popup
        ShowSuccessPopup(info.displayName);
    }

    // ========== CALLBACK: MUA THẤT BẠI ==========
    public void OnPurchaseFailed(string productId, string reason)
    {
        Debug.Log($"[Callback] Mua thất bại: {productId} - {reason}");
        ShowErrorPopup("Mua hàng thất bại", reason);
    }

    // ========== XỬ LÝ NON-CONSUMABLE ==========
    void UnlockAdsBlock()
    {
        PlayerPrefs.SetInt("AdsBlock", 1);
        PlayerPrefs.Save();
        Debug.Log("✅ Đã gỡ quảng cáo");
    }

    void UnlockSuperPack()
    {
        PlayerPrefs.SetInt("SuperPack", 1);
        PlayerPrefs.Save();
        Debug.Log("✅ Đã mở Super Pack");
    }

    void UnlockStarterPack()
    {
        PlayerPrefs.SetInt("StarterPack", 1);
        PlayerPrefs.Save();
        Debug.Log("✅ Đã mở Starter Pack");
    }

    // ========== HIỂN THỊ POPUP ==========
    void ShowSuccessPopup(string productName)
    {
        Debug.Log($"🎉 Mua thành công: {productName}");
        // TODO: Gọi UIManager.ShowPopup()
    }

    void ShowErrorPopup(string title, string message)
    {
        Debug.Log($"❌ {title}: {message}");
        // TODO: Gọi UIManager.ShowPopup()
    }

    // ========== KIỂM TRA ĐÃ MUA ==========
    public bool HasPurchased(string productId)
    {
        if (productId == IAPProductConfig.ADS_BLOCK)
            return PlayerPrefs.GetInt("AdsBlock", 0) == 1;
        if (productId == IAPProductConfig.SUPER_PACK)
            return PlayerPrefs.GetInt("SuperPack", 0) == 1;
        if (productId == IAPProductConfig.STARTER_PACK)
            return PlayerPrefs.GetInt("StarterPack", 0) == 1;
        return false;
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               