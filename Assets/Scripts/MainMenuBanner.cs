using UnityEngine;
using System.Collections;

public class MainMenuBanner : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ShowBannerAfterSceneLoad());
    }

    private IEnumerator ShowBannerAfterSceneLoad()
    {
        // Give Unity time to finish the scene transition.
        yield return new WaitForSeconds(0.2f);

        BannerAdsManager bannerAds =
            FindFirstObjectByType<BannerAdsManager>();

        if (bannerAds != null)
        {
            bannerAds.ShowBanner();
            Debug.Log("Main Menu Banner requested after scene load.");
        }
        else
        {
            Debug.LogWarning("BannerAdsManager not found.");
        }
    }
}