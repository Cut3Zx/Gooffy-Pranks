using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    public static IAPManager Instance;

    private IStoreController storeController;
    private IExtensionProvider extensionProvider;

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

    async void Start()
    {
        try
        {
            var options = new InitializationOptions().SetEnvironmentName("production");
            await UnityServices.InitializeAsync(options);
            InitializePurchasing();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Unity Services error: " + e);
        }
    }

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Lấy danh sách sản phẩm từ IAPProductConfig
        IAPProductConfig.AddProductsToBuilder(builder);

        UnityPurchasing.Initialize(this, builder);
    }

    // Hàm mua sản phẩm - gọi từ IAPBuyHandler
    public void BuyProduct(string productId)
    {
        if (storeController?.products.WithID(productId) is Product product && product.availableToPurchase)
        {
            Debug.Log("Đang mua: " + productId);
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.Log("Sản phẩm không khả dụng: " + productId);
            IAPCallbacks.Instance?.OnPurchaseFailed(productId, "Product not available");
        }
    }

    // Lấy giá sản phẩm
    public string GetPrice(string productId)
    {
        return storeController?.products.WithID(productId)?.metadata.localizedPriceString ?? "...";
    }

    // ========== CALLBACKS ==========
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        extensionProvider = extensions;
        Debug.Log("IAP sẵn sàng!");

        IAPCallbacks.Instance?.OnInitializeSuccess();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("IAP lỗi: " + error);
        IAPCallbacks.Instance?.OnInitializeFailed(error.ToString());
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"IAP lỗi: {error} - {message}");
        IAPCallbacks.Instance?.OnInitializeFailed($"{error}: {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string productId = args.purchasedProduct.definition.id;
        Debug.Log("Mua thành công: " + productId);

        IAPCallbacks.Instance?.OnPurchaseSuccess(productId);

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log($"Mua thất bại: {product.definition.id} - {reason}");
        IAPCallbacks.Instance?.OnPurchaseFailed(product.definition.id, reason.ToString());
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"Mua thất bại: {product.definition.id} - {failureDescription.reason}");
        IAPCallbacks.Instance?.OnPurchaseFailed(product.definition.id, failureDescription.reason.ToString());
    }
}