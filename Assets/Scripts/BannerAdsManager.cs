using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class BannerAdsManager : MonoBehaviour
{
    private static BannerAdsManager instance;

    public static BannerAdsManager Instance
    {
        get { return instance; }
    }

    private BannerView bannerView;

    private bool bannerShouldBeVisible = false;

    // Google official Android test Banner Ad Unit ID.
    private const string TestAdUnitId =
        "ca-app-pub-3940256099942544/6300978111";

    private void Awake()
    {
        // Prevent duplicate Banner Ads Managers.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Keep the manager alive between scenes.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(WaitForConsent());
    }

    private System.Collections.IEnumerator WaitForConsent()
    {
        while (!ConsentManager.AdsCanBeRequested)
        {
            yield return null;
        }

        Debug.Log(
            "Consent ready. Initializing Banner Ads."
        );

        InitializeBannerAds();
    }

    private void InitializeBannerAds()
    {
        MobileAds.Initialize(
            (InitializationStatus initStatus) =>
            {
                Debug.Log(
                    "Google Mobile Ads initialized for Banner."
                );

                LoadBannerAd();
            }
        );
    }

    public void LoadBannerAd()
    {
        // Destroy the old banner before creating a new one.
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

        // Get the device's safe width.
        int deviceWidth =
            MobileAds.Utils.GetDeviceSafeWidth();

        // Create an anchored adaptive banner.
        AdSize adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(
                deviceWidth
            );

        bannerView = new BannerView(
            TestAdUnitId,
            adaptiveSize,
            AdPosition.Bottom
        );

        Debug.Log(
            "Loading Banner Ad..."
        );

        AdRequest request = new AdRequest();

        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log(
                "Banner Ad loaded successfully."
            );

            // Keep the banner hidden unless a screen
            // specifically asks for it.
            if (bannerShouldBeVisible)
            {
                bannerView.Show();
            }
            else
            {
                bannerView.Hide();
            }
        };

        bannerView.OnBannerAdLoadFailed +=
            (LoadAdError error) =>
            {
                Debug.LogError(
                    "Banner Ad failed to load: " +
                    error
                );
            };

        bannerView.LoadAd(request);
    }

    public void ShowBanner()
    {
        bannerShouldBeVisible = true;

        if (bannerView != null)
        {
            bannerView.Show();
            Debug.Log("Banner Ad shown.");
        }
        else
        {
            Debug.Log(
                "Banner Ad is not loaded. Reloading Banner Ad..."
            );

            LoadBannerAd();
        }
    }

    public void HideBanner()
    {
        bannerShouldBeVisible = false;

        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;

            Debug.Log(
                "Banner Ad hidden and destroyed."
            );
        }
    }

    private void OnDestroy()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
    }
}