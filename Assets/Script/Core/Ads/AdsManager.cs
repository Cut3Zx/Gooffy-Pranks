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

    // Biến kiểm tra đã mua gỡ quảng cáo chưa
    private bool adsDisabled = false;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Kiểm tra xem đã mua gỡ quảng cáo chưa
        CheckAdsStatus();
    }

    private void Start()
    {
        // Chỉ khởi tạo ads nếu chưa gỡ
        if (!adsDisabled)
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
        else
        {
            Debug.Log("🚫 Quảng cáo đã bị vô hiệu hóa");
        }
    }

    // --------------------------
    // Kiểm tra trạng thái quảng cáo
    // --------------------------
    private void CheckAdsStatus()
    {
        adsDisabled = PlayerPrefs.GetInt("AdsBlock", 0) == 1;
    }

    // --------------------------
    // 🚫 TẮT QUẢNG CÁO (khi mua AdsBlock)
    // --------------------------
    public void DisableAds()
    {
        adsDisabled = true;

        // Ẩn banner nếu đang hiển thị
        if (bannerView != null)
        {
            bannerView.Hide();
            bannerView.Destroy();
            bannerView = null;
        }

        // Hủy interstitial
        if (interAd != null)
        {
            interAd.Destroy();
            interAd = null;
        }

        Debug.Log("🚫 ĐÃ TẮT TẤT CẢ QUẢNG CÁO!");
    }

    // --------------------------
    // Banner Ad
    // --------------------------
    private void LoadBannerAd()
    {
        if (adsDisabled) return; // Không load nếu đã tắt

        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest();
        bannerView.OnBannerAdLoaded += () => Debug.Log("✅ Banner ad loaded.");
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) => Debug.Log("❌ Banner failed: " + error.GetMessage());

        bannerView.LoadAd(request);
    }

    // Interstitial Ad

    private void LoadInterstitialAd()
    {
        if (adsDisabled) return; // Không load nếu đã tắt

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
        if (adsDisabled)
        {
            Debug.Log("🚫 Quảng cáo đã bị tắt");
            return;
        }

        if (interAd != null && interAd.CanShowAd())
        {
            interAd.Show();
        }
        else
        {
            Debug.Log("⚠️ Interstitial ad not ready");
        }
    }

    // Rewarded Ad
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
    // 🏆 PHẦN THƯỞNG SAU KHI XEM QUẢNG CÁO

    private void AddHintReward()
    {
        // Sử dụng ResourceManager thay vì PlayerPrefs trực tiếp
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.AddHints(1);
        }
        else
        {
            // Fallback nếu chưa có ResourceManager
            int currentHints = PlayerPrefs.GetInt("Hints", 0);
            currentHints++;
            PlayerPrefs.SetInt("Hints", currentHints);
            PlayerPrefs.Save();
            Debug.Log($"🎁 +1 Hint! Total hints now: {currentHints}");
        }
    }

    //  KIỂM TRA TRẠNG THÁI
    public bool AreAdsDisabled()
    {
        return adsDisabled;
    }
}