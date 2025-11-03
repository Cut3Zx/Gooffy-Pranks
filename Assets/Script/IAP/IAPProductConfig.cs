using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public static class IAPProductConfig
{
    // ID s?n ph?m
    public const string ADS_BLOCK = "AdsBlock";
    public const string SUPER_PACK = "SuperPack";
    public const string STARTER_PACK = "StarterPack";
    public const string HINT_3 = "Hint_3";
    public const string HINT_10 = "Hint_10";
    public const string HINT_20 = "Hint_20";
    public const string COIN_10 = "COIN_10";
    public const string COIN_100 = "COIN_100";
    public const string COIN_500 = "COIN_500";

    // Thêm s?n ph?m vào builder
    public static void AddProductsToBuilder(ConfigurationBuilder builder)
    {
        // Non-Consumable
        builder.AddProduct(ADS_BLOCK, ProductType.NonConsumable);
        builder.AddProduct(SUPER_PACK, ProductType.NonConsumable);
        builder.AddProduct(STARTER_PACK, ProductType.NonConsumable);

        // Consumable
        builder.AddProduct(HINT_3, ProductType.Consumable);
        builder.AddProduct(HINT_10, ProductType.Consumable);
        builder.AddProduct(HINT_20, ProductType.Consumable);
        builder.AddProduct(COIN_10, ProductType.Consumable);
        builder.AddProduct(COIN_100, ProductType.Consumable);
        builder.AddProduct(COIN_500, ProductType.Consumable);
    }

    // L?y thông tin s?n ph?m
    public static ProductInfo GetProductInfo(string productId)
    {
        switch (productId)
        {
            case ADS_BLOCK: return new ProductInfo("G? Qu?ng Cáo", 0, 0);
            case SUPER_PACK: return new ProductInfo("Super Pack", 1000, 50);
            case STARTER_PACK: return new ProductInfo("Starter Pack", 500, 20);
            case HINT_3: return new ProductInfo("3 G?i Ý", 0, 3);
            case HINT_10: return new ProductInfo("10 G?i Ý", 0, 10);
            case HINT_20: return new ProductInfo("20 G?i Ý", 0, 20);
            case COIN_10: return new ProductInfo("10 Coins", 10, 0);
            case COIN_100: return new ProductInfo("100 Coins", 100, 0);
            case COIN_500: return new ProductInfo("500 Coins", 500, 0);
            default: return new ProductInfo("Unknown", 0, 0);
        }
    }

    public struct ProductInfo
    {
        public string displayName;
        public int coinAmount;
        public int hintAmount;

        public ProductInfo(string name, int coins, int hints)
        {
            displayName = name;
            coinAmount = coins;
            hintAmount = hints;
        }
    }
}

