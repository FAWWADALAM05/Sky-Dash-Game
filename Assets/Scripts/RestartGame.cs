using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartGame : MonoBehaviour
{
    public AudioClip buttonClickSound;

    private static int restartCount = 0;

    public void Restart()
    {
        restartCount++;

        AudioSource audioSource = GetComponent<AudioSource>();

        if (MainMenuSettings.IsSoundEnabled() &&
            audioSource != null &&
            buttonClickSound != null)
        {
            audioSource.clip = buttonClickSound;
            audioSource.Play();

            StartCoroutine(
                RestartAfterSound(audioSource.clip.length)
            );
        }
        else
        {
            RestartGameNow();
        }
    }

    private IEnumerator RestartAfterSound(float delay)
    {
        Time.timeScale = 1f;

        yield return new WaitForSecondsRealtime(delay);

        RestartGameNow();
    }

    private void RestartGameNow()
    {
        Time.timeScale = 1f;

        // Show an interstitial every 3rd restart.
        if (restartCount % 3 == 0)
        {
            InterstitialAdsManager interstitialAds =
                FindFirstObjectByType<InterstitialAdsManager>();

            if (interstitialAds != null)
            {
                Debug.Log(
                    "3rd restart reached. Showing Interstitial."
                );

                interstitialAds.ShowInterstitialAd(
                    RestartScene
                );

                return;
            }
        }

        // Normal restart when no ad is needed.
        RestartScene();
    }

    private void RestartScene()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}