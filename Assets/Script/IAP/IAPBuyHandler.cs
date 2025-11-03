using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPBuyHandler : MonoBehaviour
{
    public static IAPBuyHandler Instance;

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

    // ========== NON-CONSUMABLE ==========
    public void BuyAdsBlock()
    {
        IAPManager.Instance?.BuyProduct(IAPProductConfig.ADS_BLOCK);
    }

    public void BuySuperPack()
    {
        IAPManager.Instance?.BuyProduct(IAPProductConfig.SUPER_PACK);
    }

    public void BuyStarterPack()
    {
        IAPManager.Instance?.BuyProduct(IAPProductConfig.STARTER_PACK);
    }

    // ========== CONSUMABLE - HINTS ==========
    public void BuyHint3()
    {
        IAPManager.Instance?.BuyProduct(IAPProductConfig.HINT_3);
    }

    public void BuyHint10()
    {
        IAPManager.Instance?.BuyProduct(IAPProductConfig.HINT_10);
    }

    public void BuyHint20()
    {
        IAPManager.Instance?.BuyProduct(IAPProductConfig.HINT_20);
    }

    // ========== CONSUMABLE - COINS ==========
    public void BuyCoin10()
    {
        IAPManager.Instance?.BuyProduct(IAPProductConfig.COIN_10);
    }

    public void BuyCoin100()
    {
        IAPManager.Instance?.BuyProduct(IAPProductConfig.COIN_100);
    }

    public void BuyCoin500()
    {
        IAPManager.Instance?.BuyProduct(IAPProductConfig.COIN_500);
    }
}