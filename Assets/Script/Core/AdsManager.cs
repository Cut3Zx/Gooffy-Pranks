using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    private string bannerId = "ca-app-pub-3940256099942544/6300978111";
    private string interId = "ca-app-pub-3940256099942544/1033173712";
    private string rewardId = "ca-app-pub-3940256099942544/5224354917";

    private BannerView bannerView;
    private InterstitialAd interAd;
    private RewardedAd rewardedAd;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        // Initialize the google mobile ads SDK
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Admob SDK Initilized");
            //Load ads after initialization
            LoadBannerAd();
            LoadInterstitialAd();
            LoadRewardedAd();

        });
    }

    private void LoadBannerAd()
    {
        // Create a banner view at the bottom of the screen
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request
        AdRequest request = new AdRequest();

        // Register event handlers for the banner ad
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner ad loaded.");
        };

        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.Log("Banner ad failed to load: " + error.GetMessage());
        };

        // Load the banner ad
        bannerView.LoadAd(request);


    }

    private void LoadInterstitialAd()
    {

        // Clean up any existing interstitial ad
        if (interAd != null)
        {
            interAd.Destroy();
            interAd = null;
        }

        // Load a new interstitial ad
        InterstitialAd.Load(interId, new AdRequest(), (InterstitialAd ad, LoadAdError error) => {

            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load: " + error?.GetMessage());
                return;
            }
            ;

            interAd = ad;
            Debug.Log("Interstitial ad loaded");

            // Register ad events
            interAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial ad closed");
                LoadInterstitialAd(); // Preload the next ad
            };

            interAd.OnAdFullScreenContentFailed += (error) =>
            {
                Debug.Log("Interstitial ad failed to show: " + error.GetMessage());
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
            Debug.Log("Interstitial ad not ready");
        }
    }

    private void LoadRewardedAd()
    {

        // Clean up any existing rewarded ad
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;

        }

        // Load a new rewarded ad
        RewardedAd.Load(rewardId, new AdRequest(), (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load:" + error?.GetMessage());
                return;
            }

            rewardedAd = ad;
            Debug.Log("Rewarded ad loaded.");

            // Register ad events
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.LogError("Rewarded ad closed.");
                LoadRewardedAd();
            };
            rewardedAd.OnAdFullScreenContentFailed += (error) =>
            {
                Debug.LogError("Rewarded ad failed to show:" + error.GetMessage());
            };
        });


    }


    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"User earned reward: {reward.Amount} {reward.Type}");
                //+1 hint
                

            });
        }
        else
        {
            Debug.Log("Rewarded ad not ready");
        }
    }

}