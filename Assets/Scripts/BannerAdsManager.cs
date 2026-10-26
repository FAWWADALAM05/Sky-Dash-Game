using UnityEngine;
using CrazyGames;

#if !UNITY_WEBGL || UNITY_EDITOR
using GoogleMobileAds;
using GoogleMobileAds.Api;
#endif

public class BannerAdsManager : MonoBehaviour
{
    private static BannerAdsManager instance;

    public static BannerAdsManager Instance
    {
        get { return instance; }
    }

    // =========================================================
    // CRAZYGAMES BANNER
    // =========================================================

    [Header("CrazyGames")]
    [SerializeField]
    private CrazyBanner crazyBannerPrefab;

#if UNITY_WEBGL && !UNITY_EDITOR

    private CrazyBanner crazyBanner;

#endif

    // =========================================================
    // ADMOB BANNER
    // =========================================================

#if !UNITY_WEBGL || UNITY_EDITOR

    private BannerView bannerView;

    // Google official Android test Banner Ad Unit ID.
    private const string TestAdUnitId =
        "ca-app-pub-1574762395478706/2852873471";

#endif

    // =========================================================
    // COMMON
    // =========================================================

    private bool bannerShouldBeVisible = false;


    // =========================================================
    // AWAKE
    // =========================================================

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


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR

        Debug.Log(
            "CrazyGames WebGL Banner Manager ready."
        );

#else

        StartCoroutine(WaitForConsent());

#endif
    }


    // =========================================================
    // ADMOB - WAIT FOR CONSENT
    // =========================================================

#if !UNITY_WEBGL || UNITY_EDITOR

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


    // =========================================================
    // ADMOB - INITIALIZE
    // =========================================================

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


    // =========================================================
    // ADMOB - LOAD BANNER
    // =========================================================

    public void LoadBannerAd()
    {
        // Destroy the old banner before creating a new one.
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

        int deviceWidth =
            MobileAds.Utils.GetDeviceSafeWidth();

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

#endif


    // =========================================================
    // SHOW BANNER
    // =========================================================

    public void ShowBanner()
    {
        bannerShouldBeVisible = true;

#if UNITY_WEBGL && !UNITY_EDITOR

        ShowCrazyGamesBanner();

#else

        if (bannerView != null)
        {
            bannerView.Show();

            Debug.Log(
                "AdMob Banner shown."
            );
        }
        else
        {
            Debug.Log(
                "AdMob Banner is not loaded. " +
                "Reloading Banner Ad..."
            );

            LoadBannerAd();
        }

#endif
    }


    // =========================================================
    // HIDE BANNER
    // =========================================================

    public void HideBanner()
    {
        bannerShouldBeVisible = false;

#if UNITY_WEBGL && !UNITY_EDITOR

        HideCrazyGamesBanner();

#else

        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;

            Debug.Log(
                "AdMob Banner hidden and destroyed."
            );
        }

#endif
    }


    // =========================================================
    // CRAZYGAMES - SHOW BANNER
    // =========================================================

#if UNITY_WEBGL && !UNITY_EDITOR

    private void ShowCrazyGamesBanner()
    {
        if (!CrazySDK.IsAvailable)
        {
            Debug.LogWarning(
                "CrazyGames SDK is not available."
            );

            return;
        }

        if (!CrazySDK.IsInitialized)
        {
            Debug.LogWarning(
                "CrazyGames SDK is not initialized."
            );

            return;
        }

        if (crazyBannerPrefab == null)
        {
            Debug.LogError(
                "CrazyBanner Prefab is not assigned " +
                "in BannerAdsManager."
            );

            return;
        }

        // Create the CrazyGames banner only once.
        if (crazyBanner == null)
        {
            crazyBanner =
                Instantiate(crazyBannerPrefab);

            DontDestroyOnLoad(
                crazyBanner.gameObject
            );

            ConfigureCrazyBanner();
        }

        // Show banner.
        crazyBanner.gameObject.SetActive(true);

        // Tell CrazyGames to refresh visible banners.
        CrazySDK.Banner.RefreshBanners();

        Debug.Log(
            "CrazyGames Banner shown."
        );
    }


    // =========================================================
    // CRAZYGAMES - CONFIGURE BANNER
    // =========================================================

    private void ConfigureCrazyBanner()
    {
        float width = Screen.width;

        if (width >= 1000f)
        {
            // Desktop / large screen.
            crazyBanner.Size =
                CrazyBanner.BannerSize.Leaderboard_728x90;
        }
        else if (width >= 600f)
        {
            // Tablet / medium screen.
            crazyBanner.Size =
                CrazyBanner.BannerSize.Main_Banner_468x60;
        }
        else
        {
            // Mobile screen.
            crazyBanner.Size =
                CrazyBanner.BannerSize.Mobile_320x50;
        }

        // Bottom-center.
// The Banner prefab is anchored to Bottom Center.
crazyBanner.Position =
    new Vector2(0f, 45f);
    }


    // =========================================================
    // CRAZYGAMES - HIDE BANNER
    // =========================================================

    private void HideCrazyGamesBanner()
    {
        if (crazyBanner != null)
        {
            crazyBanner.gameObject.SetActive(false);

            CrazySDK.Banner.RefreshBanners();

            Debug.Log(
                "CrazyGames Banner hidden."
            );
        }
    }

#endif


    // =========================================================
    // ON DESTROY
    // =========================================================

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }

#if !UNITY_WEBGL || UNITY_EDITOR

        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

#endif
    }
}