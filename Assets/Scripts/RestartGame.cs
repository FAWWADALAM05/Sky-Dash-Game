using UnityEngine;
using UnityEngine.SceneManagement;
using CrazyGames;

public class RestartGame : MonoBehaviour
{
    private static int restartCount = 0;

    public void Restart()
    {
        restartCount++;

        Debug.Log(
            "Restart button clicked. Restart count: " +
            restartCount
        );

        // Show an ad on every 3rd restart.
        if (restartCount % 3 == 0)
        {
#if UNITY_WEBGL && !UNITY_EDITOR

            Debug.Log(
                "3rd restart reached. Showing CrazyGames Midgame Ad."
            );

            if (CrazySDK.IsAvailable &&
                CrazySDK.IsInitialized)
            {
                CrazySDK.Ad.RequestAd(
                    CrazyAdType.Midgame,

                    // Ad started
                    () =>
                    {
                        Debug.Log(
                            "CrazyGames Midgame Ad started."
                        );
                    },

                    // Ad error
                    error =>
                    {
                        Debug.LogWarning(
                            "CrazyGames Midgame Ad error: " +
                            error
                        );

                        RestartScene();
                    },

                    // Ad finished
                    () =>
                    {
                        Debug.Log(
                            "CrazyGames Midgame Ad finished."
                        );

                        RestartScene();
                    }
                );
            }
            else
            {
                Debug.LogWarning(
                    "CrazyGames SDK not ready. Restarting."
                );

                RestartScene();
            }

#else

            InterstitialAdsManager interstitialAds =
                FindFirstObjectByType<InterstitialAdsManager>();

            if (interstitialAds != null)
            {
                Debug.Log(
                    "3rd restart reached. Showing AdMob Interstitial."
                );

                interstitialAds.ShowInterstitialAd(
                    RestartScene
                );
            }
            else
            {
                Debug.LogWarning(
                    "InterstitialAdsManager not found. Restarting."
                );

                RestartScene();
            }

#endif

            return;
        }

        // Normal restart when no ad is needed.
        RestartScene();
    }

    private void RestartScene()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.buildIndex
        );
    }
}