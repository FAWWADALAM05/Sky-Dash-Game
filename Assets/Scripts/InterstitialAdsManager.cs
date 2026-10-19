using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class InterstitialAdsManager : MonoBehaviour
{
    private static InterstitialAdsManager instance;

    private InterstitialAd interstitialAd;

    // Google official Android Interstitial Test Ad Unit ID.
    private const string TestAdUnitId =
        "ca-app-pub-1574762395478706/8154668599";

    private void Awake()
    {
        Debug.Log("INTERSTITIAL MANAGER AWAKE.");
        // Prevent duplicate managers when the Game scene reloads.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Keep this manager alive between scene changes.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("INTERSTITIAL MANAGER START.");

        StartCoroutine(WaitForConsent());
    }

    private System.Collections.IEnumerator WaitForConsent()
    {
        Debug.Log(
        "INTERSTITIAL CHECKING CONSENT: " +
        ConsentManager.AdsCanBeRequested
    );
        while (!ConsentManager.AdsCanBeRequested)
        {
            yield return null;
        }

        Debug.Log(
            "Consent ready. Initializing Interstitial Ads."
        );

        InitializeAds();
    }

    private void InitializeAds()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log(
                "Google Mobile Ads initialized for Interstitial."
            );

            LoadInterstitialAd();
        });
    }

    public void LoadInterstitialAd()
    {
        // Destroy old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        AdRequest request = new AdRequest();

        Debug.Log("Loading Interstitial Ad...");

        InterstitialAd.Load(
            TestAdUnitId,
            request,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError(
                        "Interstitial Ad failed to load: " +
                        error
                    );

                    return;
                }

                Debug.Log(
                    "Interstitial Ad loaded successfully."
                );

                interstitialAd = ad;

                RegisterInterstitialAdEvents(
                    interstitialAd
                );
            }
        );
    }

    public void ShowInterstitialAd(
        Action afterAdClosed = null
    )
    {
        if (interstitialAd != null &&
            interstitialAd.CanShowAd())
        {
            Debug.Log(
                "Showing Interstitial Ad..."
            );

            // The ad object is one-time use.
            InterstitialAd adToShow =
                interstitialAd;

            interstitialAd = null;

            adToShow.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log(
                    "Interstitial Ad closed."
                );

                // Load the next ad.
                LoadInterstitialAd();

                // Continue the action that was waiting
                // for the ad to finish.
                if (afterAdClosed != null)
                {
                    afterAdClosed.Invoke();
                }
            };

            adToShow.OnAdFullScreenContentFailed +=
                (AdError error) =>
                {
                    Debug.LogError(
                        "Interstitial Ad failed to show: " +
                        error
                    );

                    LoadInterstitialAd();

                    // If the ad fails, continue normally.
                    if (afterAdClosed != null)
                    {
                        afterAdClosed.Invoke();
                    }
                };

            adToShow.Show();
        }
        else
        {
            Debug.LogWarning(
                "Interstitial Ad is not ready."
            );

            // If no ad is available, continue normally.
            if (afterAdClosed != null)
            {
                afterAdClosed.Invoke();
            }

            // Try loading another ad.
            LoadInterstitialAd();
        }
    }

    private void RegisterInterstitialAdEvents(
        InterstitialAd ad
    )
    {
        ad.OnAdFullScreenContentFailed +=
            (AdError error) =>
            {
                Debug.LogError(
                    "Interstitial Ad failed: " +
                    error
                );

                LoadInterstitialAd();
            };
    }
}