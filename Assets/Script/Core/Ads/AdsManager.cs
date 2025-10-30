using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    private string bannerId = "ca-app-pub-1945244255127558/2361559821";
    private string interId = "ca-app-pub-1945244255127558/9815160974";
    private string rewardId = "ca-app-pub-1945244255127558/1375781213";

    private BannerView bannerView;
    private InterstitialAd interAd;
    private RewardedAd rewardedAd;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Initialize the Google Mobile Ads SDK
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("✅ Admob SDK Initialized");

            // Load ads after initialization
            LoadBannerAd();
            LoadInterstitialAd();
            LoadRewardedAd();
        });
    }

    // --------------------------
    // Banner Ad
    // --------------------------
    private void LoadBannerAd()
    {
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest();
        bannerView.OnBannerAdLoaded += () => Debug.Log("✅ Banner ad loaded.");
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) => Debug.Log("❌ Banner failed: " + error.GetMessage());

        bannerView.LoadAd(request);
    }

    // --------------------------
    // Interstitial Ad
    // --------------------------
    private void LoadInterstitialAd()
    {
        if (interAd != null)
        {
            interAd.Destroy();
            interAd = null;
        }

        InterstitialAd.Load(interId, new AdRequest(), (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("❌ Interstitial failed to load: " + error?.GetMessage());
                return;
            }

            interAd = ad;
            Debug.Log("✅ Interstitial ad loaded");

            interAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("ℹ️ Interstitial ad closed — reloading...");
                LoadInterstitialAd();
            };
        });
    }

    public void ShowInterstialAd()
    {
        if (interAd != null && interAd.CanShowAd())
        {
            interAd.Show();
        }
        else
        {
            Debug.Log("⚠️ Interstitial ad not ready");
        }
    }

    // --------------------------
    // Rewarded Ad
    // --------------------------
    private void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        RewardedAd.Load(rewardId, new AdRequest(), (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("❌ Rewarded ad failed to load: " + error?.GetMessage());
                return;
            }

            rewardedAd = ad;
            Debug.Log("✅ Rewarded ad loaded.");

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("ℹ️ Rewarded ad closed — reloading...");
                LoadRewardedAd();
            };
        });
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"🏆 User earned reward: {reward.Amount} {reward.Type}");

                // 🏅 GỌI PHẦN THƯỞNG SAU KHI XEM XONG QUẢNG CÁO
                AddHintReward();
            });
        }
        else
        {
            Debug.Log("⚠️ Rewarded ad not ready");
        }
    }

    // --------------------------
    // 🏆 PHẦN THƯỞNG SAU KHI XEM QUẢNG CÁO
    // --------------------------
    private void AddHintReward()
    {
        int currentHints = PlayerPrefs.GetInt("hintCount", 0);
        currentHints++;
        PlayerPrefs.SetInt("hintCount", currentHints);
        PlayerPrefs.Save();

        Debug.Log($"🎁 +1 Hint! Total hints now: {currentHints}");
    }
}
