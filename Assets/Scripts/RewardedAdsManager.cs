using UnityEngine;
using CrazyGames;

#if !UNITY_WEBGL || UNITY_EDITOR
using GoogleMobileAds;
using GoogleMobileAds.Api;
#endif

public class RewardedAdsManager : MonoBehaviour
{
#if !UNITY_WEBGL || UNITY_EDITOR

    private RewardedAd rewardedAd;

    // Google official Android Rewarded Test Ad Unit ID.
    private const string TestAdUnitId =
        "ca-app-pub-1574762395478706/1555802351";

#endif

    // True when the rewarded ad was opened to give coins.
    private bool rewardCoinsPending = false;

    // Reward amount for Watch Ad buttons.
    private const int REWARD_COINS = 50;


    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR

        Debug.Log("CrazyGames WebGL Rewarded Ads ready.");

#else

        StartCoroutine(WaitForConsent());

#endif
    }


#if !UNITY_WEBGL || UNITY_EDITOR

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
        MobileAds.Initialize(
            (InitializationStatus initStatus) =>
            {
                Debug.Log(
                    "Google Mobile Ads initialized."
                );

                LoadRewardedAd();
            }
        );
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

#endif


    // =========================================================
    // GAME OVER - WATCH AD & CONTINUE
    // =========================================================

    public void ShowRewardedAd()
    {
        Debug.Log(
            "GAME OVER WATCH AD BUTTON CLICKED!"
        );

        // This is NOT a coin reward.
        rewardCoinsPending = false;

        RequestRewardedAd();
    }


    // =========================================================
    // MAIN MENU / SHOP - WATCH AD FOR 50 COINS
    // =========================================================

    public void ShowRewardedAdForCoins()
    {
        Debug.Log(
            "WATCH AD FOR 50 COINS BUTTON CLICKED!"
        );

        // This ad gives 50 coins.
        rewardCoinsPending = true;

        RequestRewardedAd();
    }


    // =========================================================
    // REQUEST REWARDED AD
    // =========================================================

    private void RequestRewardedAd()
    {
#if UNITY_WEBGL && !UNITY_EDITOR

        if (!CrazySDK.IsAvailable)
        {
            Debug.LogWarning(
                "CrazyGames SDK is not available."
            );

            rewardCoinsPending = false;
            return;
        }

        if (!CrazySDK.IsInitialized)
        {
            Debug.LogWarning(
                "CrazyGames SDK is not initialized."
            );

            rewardCoinsPending = false;
            return;
        }

        Debug.Log(
            "Showing CrazyGames Rewarded Ad..."
        );

        CrazySDK.Ad.RequestAd(
            CrazyAdType.Rewarded,

            // Ad started
            () =>
            {
                Debug.Log(
                    "CrazyGames Rewarded Ad started."
                );
            },

            // Ad error
            error =>
            {
                Debug.LogWarning(
                    "CrazyGames Rewarded Ad error: " +
                    error
                );

                rewardCoinsPending = false;
            },

            // Ad finished
            () =>
            {
                Debug.Log(
                    "CrazyGames Rewarded Ad finished."
                );

                HandleRewardedAdCompleted();
            }
        );

#else

        if (rewardedAd != null &&
            rewardedAd.CanShowAd())
        {
            Debug.Log(
                "Showing AdMob Rewarded Ad..."
            );

            rewardedAd.Show(
                (Reward reward) =>
                {
                    Debug.Log(
                        "Reward earned: " +
                        reward.Amount +
                        " " +
                        reward.Type
                    );

                    HandleRewardedAdCompleted();
                }
            );
        }
        else
        {
            Debug.LogWarning(
                "Rewarded Ad is not ready yet."
            );

            LoadRewardedAd();
        }

#endif
    }


    // =========================================================
    // REWARDED AD COMPLETED
    // =========================================================

    private void HandleRewardedAdCompleted()
    {
        if (rewardCoinsPending)
        {
            GiveCoinsReward();

            rewardCoinsPending = false;

            return;
        }

        ContinueAfterReward();
    }


    // =========================================================
    // GIVE 50 COINS
    // =========================================================

    private void GiveCoinsReward()
    {
        Debug.Log(
            "Giving " +
            REWARD_COINS +
            " coins reward."
        );


        // SHOP
        // Uses the existing ShopCurrencyManager.
        // This updates the Shop coin text immediately.
        if (ShopCurrencyManager.Instance != null)
        {
            ShopCurrencyManager.Instance.AddCoins(
                REWARD_COINS
            );

            Debug.Log(
                "Shop received +" +
                REWARD_COINS +
                " coins."
            );

            return;
        }


        // MAIN MENU
        // Fallback when ShopCurrencyManager does not exist.
        int totalCoins =
            PlayerPrefs.GetInt(
                "TotalCoins",
                0
            );

        int newCoins =
            totalCoins +
            REWARD_COINS;

        PlayerPrefs.SetInt(
            "TotalCoins",
            newCoins
        );

        PlayerPrefs.Save();


        MainMenuCoins mainMenuCoins =
            FindFirstObjectByType<MainMenuCoins>();

        if (mainMenuCoins != null)
        {
            mainMenuCoins.RefreshCoins();

            Debug.Log(
                "Main Menu received +" +
                REWARD_COINS +
                " coins."
            );
        }
        else
        {
            Debug.Log(
                "50 coins saved. MainMenuCoins not found."
            );
        }
    }


    // =========================================================
    // GAME OVER - CONTINUE GAME
    // =========================================================

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
                "PlayerCollision not found. " +
                "Cannot continue game."
            );
        }
    }


#if !UNITY_WEBGL || UNITY_EDITOR

    // =========================================================
    // ADMOB EVENTS
    // =========================================================

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

                rewardCoinsPending = false;

                LoadRewardedAd();
            };
    }

#endif
}