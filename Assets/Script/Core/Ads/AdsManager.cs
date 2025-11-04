using GoogleMobileAds.Api;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    private string bannerId = "ca-app-pub-3940256099942544/6300978111";
    private string interId = "ca-app-pub-1945244255127558/9815160974";
    private string rewardId = "ca-app-pub-1945244255127558/1375781213";

    private BannerView bannerView;
    private InterstitialAd interAd;
    private RewardedAd rewardedAd;
    private int levelsSinceLastAd
    {
        get => PlayerPrefs.GetInt("levelsSinceLastAd", 0);
        set
        {
            PlayerPrefs.SetInt("levelsSinceLastAd", value);
            PlayerPrefs.Save();
        }
    }


    private bool adsInitialized = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        

    }

    private void Start()
    {
        //// ✅ Chờ UMP xử lý consent xong mới khởi tạo quảng cáo
        //if (ConsentManager.Instance != null && !ConsentManager.Instance.IsReady)
        //{
        //    ConsentManager.Instance.OnConsentFlowFinished += OnConsentReadyThenInit;
        //}
        //else
        //{
        //    InitializeAds();
        //}
    }

    private void OnConsentReadyThenInit()
    {
        ConsentManager.Instance.OnConsentFlowFinished -= OnConsentReadyThenInit;
        InitializeAds();
    }

    private void InitializeAds()
    {
        //if (adsInitialized) return;
        //adsInitialized = true;

        //MobileAds.Initialize(initStatus =>
        //{
        //    Debug.Log("✅ Admob SDK Initialized");

        //    // Banner luôn load (hiển thị suốt game)
        //    LoadBannerAd();

        //    // Chuẩn bị interstitial + rewarded (sẽ gọi sau khi đủ điều kiện)
        //    LoadInterstitialAd();
        //    LoadRewardedAd();
        //});
    }


    // --------------------------
    // Banner Ad
    // --------------------------
    private void LoadBannerAd()
    {
        //if (bannerView != null)
        //{
        //    bannerView.Destroy();
        //    bannerView = null;
        //}

        //bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);

        //AdRequest request = BuildRequest();

        //bannerView.OnBannerAdLoaded += () =>
        //{
        //    Debug.Log("✅ Banner loaded");
        //    bannerView.Show(); // 👈 Thêm dòng này
        //};

        //bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        //{
        //    Debug.LogWarning("❌ Banner failed: " + error.GetMessage());
        //};

        //bannerView.LoadAd(request);
    }


    // --------------------------
    // Interstitial Ad
    // --------------------------
    private void LoadInterstitialAd()
    {
        //InterstitialAd.Load(interId, BuildRequest(), (InterstitialAd ad, LoadAdError error) =>
        //{
        //    if (error != null || ad == null)
        //    {
        //        Debug.LogWarning("❌ Interstitial load fail: " + error?.GetMessage());
        //        return;
        //    }

        //    interAd = ad;
        //    interAd.OnAdFullScreenContentClosed += () =>
        //    {
        //        Debug.Log("ℹ️ Interstitial closed — reloading...");
        //        LoadInterstitialAd();
        //    };
        //});
    }

    public void ShowInterstitialAd()
    {
        //if (interAd != null && interAd.CanShowAd())
        //{
        //    interAd.Show();
        //}
        //else
        //{
        //    Debug.Log("⚠️ Interstitial not ready");
        //}
    }

    // --------------------------
    // Rewarded Ad
    // --------------------------
    private void LoadRewardedAd()
    {
        //RewardedAd.Load(rewardId, BuildRequest(), (RewardedAd ad, LoadAdError error) =>
        //{
        //    if (error != null || ad == null)
        //    {
        //        Debug.LogWarning("❌ Rewarded load fail: " + error?.GetMessage());
        //        return;
        //    }

        //    rewardedAd = ad;
        //    rewardedAd.OnAdFullScreenContentClosed += () =>
        //    {
        //        Debug.Log("ℹ️ Rewarded ad closed — reloading...");
        //        LoadRewardedAd();
        //    };
        //});
    }

    public void ShowRewardedAd()
    {
        //if (rewardedAd != null && rewardedAd.CanShowAd())
        //{
        //    rewardedAd.Show((Reward reward) =>
        //    {
        //        Debug.Log($"🏆 User earned reward: {reward.Amount} {reward.Type}");
        //        AddHintReward();
        //    });
        //}
        //else
        //{
        //    Debug.Log("⚠️ Rewarded ad not ready");
        //}
    }

    // --------------------------
    // Request Builder (có kiểm soát Consent)
    // --------------------------
    private AdRequest BuildRequest()
    {
        if (ConsentManager.Instance != null)
            return ConsentManager.Instance.BuildAdRequest();

        return new AdRequest();
    }

    // --------------------------
    // Reward handling
    // --------------------------
    private void AddHintReward()
    {
        int currentHints = PlayerPrefs.GetInt("hintCount", 0);
        currentHints++;
        PlayerPrefs.SetInt("hintCount", currentHints);
        PlayerPrefs.Save();

        Debug.Log($"🎁 +1 Hint! Total hints now: {currentHints}");
    }
    public void OnLevelCompleted(int currentLevel)
    {
        // 🟩 Banner vẫn luôn hiển thị, không bị ảnh hưởng
        //if (bannerView == null)
        //{
        //    Debug.Log("📢 Banner chưa có, tự động load lại...");
        //    LoadBannerAd();
        //}

        //// 🟥 Kiểm tra điều kiện cho Interstitial
        //if (currentLevel <= 5)
        //{
        //    Debug.Log($"🚫 Level {currentLevel} < 6 → chưa hiển thị interstitial.");
        //    return;
        //}

        //// Tăng bộ đếm
        //levelsSinceLastAd++;
        //Debug.Log($"📊 Đã thắng {levelsSinceLastAd} màn kể từ lần quảng cáo cuối. (Level hiện tại: {currentLevel})");

        //// Sau mỗi 3 màn → tung tỉ lệ
        //if (levelsSinceLastAd >= 3)
        //{
        //    levelsSinceLastAd = 0;
        //    float randomChance = UnityEngine.Random.value; // 0–1
        //    float showChance = 0.7f; // 70%

        //    if (randomChance <= showChance)
        //    {
        //        Debug.Log($"🎬 Hiển thị quảng cáo xen kẽ sau 3 level (tỉ lệ {showChance * 100}%).");

        //        if (interAd != null && interAd.CanShowAd())
        //        {
        //            interAd.Show();
        //        }
        //        else
        //        {
        //            Debug.Log("⚠️ Interstitial chưa sẵn sàng, tải lại...");
        //            LoadInterstitialAd();
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log($"🎲 Random {randomChance:F2} > {showChance} → bỏ qua quảng cáo lần này.");
        //    }
        //}
        //else
        //{
        //    Debug.Log($"⏩ Chưa đủ 3 màn (hiện {levelsSinceLastAd}/3), chưa hiển thị interstitial.");
        //}
    }



}
