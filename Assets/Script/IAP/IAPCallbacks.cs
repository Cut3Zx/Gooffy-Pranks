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
        ShowErrorPopup("IAP không khả dụng", "Không thể kết nối đến cửa hàng. Vui lòng thử lại sau.");
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

        // Phân loại lỗi và xử lý tương ứng
        if (reason.Contains("UserCancelled"))
        {
            // User tự hủy - không cần thông báo
            Debug.Log("💬 User đã hủy giao dịch");
            // Không hiển thị popup để không làm phiền user
        }
        else if (reason.Contains("PaymentDeclined"))
        {
            // Thẻ bị từ chối
            ShowErrorPopup(
                "Thanh toán thất bại",
                "Thẻ của bạn bị từ chối.\nVui lòng kiểm tra lại thông tin thanh toán hoặc thử phương thức khác."
            );
        }
        else if (reason.Contains("ServiceUnavailable") || reason.Contains("NetworkError"))
        {
            // Lỗi mạng hoặc Google Play lỗi
            ShowErrorPopup(
                "Lỗi kết nối",
                "Không thể kết nối đến Google Play.\nVui lòng kiểm tra kết nối mạng và thử lại."
            );
        }
        else if (reason.Contains("PurchasingUnavailable"))
        {
            // IAP không khả dụng trên thiết bị
            ShowErrorPopup(
                "IAP không khả dụng",
                "Tính năng mua hàng không khả dụng trên thiết bị này."
            );
        }
        else if (reason.Contains("ExistingPurchasePending"))
        {
            // Có giao dịch đang chờ xử lý
            ShowErrorPopup(
                "Giao dịch đang chờ",
                "Bạn có giao dịch đang chờ xử lý.\nVui lòng đợi trong giây lát."
            );
        }
        else if (reason.Contains("ProductUnavailable"))
        {
            // Sản phẩm không tồn tại trên Store
            ShowErrorPopup(
                "Sản phẩm không khả dụng",
                "Sản phẩm này tạm thời không khả dụng.\nVui lòng thử lại sau."
            );
        }
        else if (reason.Contains("SignatureInvalid"))
        {
            // Chữ ký không hợp lệ - có thể bị hack
            ShowErrorPopup(
                "Lỗi bảo mật",
                "Giao dịch không hợp lệ.\nVui lòng liên hệ hỗ trợ."
            );
            Debug.LogError("⚠️ CẢNH BÁO: Signature invalid - có thể bị tấn công IAP crack!");
        }
        else if (reason.Contains("DuplicateTransaction"))
        {
            // Giao dịch trùng lặp
            ShowErrorPopup(
                "Giao dịch trùng lặp",
                "Giao dịch này đã được xử lý trước đó."
            );
        }
        else if (reason.Contains("Unknown"))
        {
            // Lỗi không xác định
            ShowErrorPopup(
                "Mua hàng thất bại",
                "Đã xảy ra lỗi không xác định.\nVui lòng thử lại sau."
            );
        }
        else
        {
            // Lỗi khác
            ShowErrorPopup(
                "Mua hàng thất bại",
                $"Lỗi: {reason}\nVui lòng thử lại sau."
            );
        }
    }

    // ========== XỬ LÝ NON-CONSUMABLE ==========
    void UnlockAdsBlock()
    {
        PlayerPrefs.SetInt("AdsBlock", 1);
        PlayerPrefs.Save();
        Debug.Log("✅ Đã gỡ quảng cáo");
        if (AdsManager.Instance != null)
        AdsManager.Instance.DisableAds();
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
    }

    void ShowErrorPopup(string title, string message)
    {
        Debug.Log($"❌ {title}: {message}");     
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

    // ========== DEBUG: Hiển thị tất cả trạng thái mua hàng ==========
    [ContextMenu("Show Purchase Status")]
    public void ShowPurchaseStatus()
    {
        Debug.Log("=== TRẠNG THÁI MUA HÀNG ===");
        Debug.Log($"AdsBlock: {(HasPurchased(IAPProductConfig.ADS_BLOCK) ? "✅ Đã mua" : "❌ Chưa mua")}");
        Debug.Log($"SuperPack: {(HasPurchased(IAPProductConfig.SUPER_PACK) ? "✅ Đã mua" : "❌ Chưa mua")}");
        Debug.Log($"StarterPack: {(HasPurchased(IAPProductConfig.STARTER_PACK) ? "✅ Đã mua" : "❌ Chưa mua")}");
        Debug.Log("============================");
    }

    // ========== DEBUG: Reset trạng thái mua hàng ==========
    [ContextMenu("Reset All Purchases")]
    public void ResetAllPurchases()
    {
        PlayerPrefs.DeleteKey("AdsBlock");
        PlayerPrefs.DeleteKey("SuperPack");
        PlayerPrefs.DeleteKey("StarterPack");
        PlayerPrefs.Save();
        Debug.Log("🔄 Đã reset tất cả trạng thái mua hàng!");
    }
}