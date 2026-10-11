using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class RewardedAdsManager : MonoBehaviour
{
    private RewardedAd rewardedAd;

    // Google official Android Rewarded Test Ad Unit ID.
    private const string TestAdUnitId =
        "ca-app-pub-3940256099942544/5224354917";

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
            "Consent ready. Initializing Rewarded Ads."
        );

        InitializeAds();
    }

    private void InitializeAds()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log(
                "Google Mobile Ads initialized."
            );

            LoadRewardedAd();
        });
    }

    public void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        AdRequest request = new AdRequest();

        Debug.Log(
            "Loading Rewarded Ad..."
        );

        RewardedAd.Load(
            TestAdUnitId,
            request,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError(
                        "Rewarded Ad failed to load: " +
                        error
                    );

                    return;
                }

                Debug.Log(
                    "Rewarded Ad loaded successfully."
                );

                rewardedAd = ad;

                RegisterRewardedAdEvents(
                    rewardedAd
                );
            }
        );
    }

    public void ShowRewardedAd()
    {
        Debug.Log(
            "WATCH AD BUTTON CLICKED!"
        );

        if (rewardedAd != null &&
            rewardedAd.CanShowAd())
        {
            Debug.Log(
                "Showing Rewarded Ad..."
            );

            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(
                    "Reward earned: " +
                    reward.Amount +
                    " " +
                    reward.Type
                );

                // The actual Continue action will be connected here.
                ContinueAfterReward();
            });
        }
        else
        {
            Debug.LogWarning(
                "Rewarded Ad is not ready yet."
            );

            LoadRewardedAd();
        }
    }

    private void ContinueAfterReward()
    {
        Debug.Log(
            "Reward received. Continuing game."
        );

        PlayerCollision playerCollision =
            FindFirstObjectByType<PlayerCollision>();

        if (playerCollision != null)
        {
            playerCollision.ContinueGame();
        }
        else
        {
            Debug.LogError(
                "PlayerCollision not found. Cannot continue game."
            );
        }
    }

    private void RegisterRewardedAdEvents(
        RewardedAd ad
    )
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log(
                "Rewarded Ad closed."
            );

            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed +=
            (AdError error) =>
            {
                Debug.LogError(
                    "Rewarded Ad failed to show: " +
                    error
                );

                LoadRewardedAd();
            };
    }
}