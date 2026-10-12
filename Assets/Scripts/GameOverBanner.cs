using UnityEngine;

public class GameOverBanner : MonoBehaviour
{
    private void OnEnable()
    {
        BannerAdsManager bannerAds =
            BannerAdsManager.Instance;

        if (bannerAds != null)
        {
            bannerAds.ShowBanner();
            Debug.Log("Game Over Banner shown.");
        }
        else
        {
            Debug.LogWarning("BannerAdsManager instance not found.");
        }
    }

    private void OnDisable()
    {
        BannerAdsManager bannerAds =
            BannerAdsManager.Instance;

        if (bannerAds != null)
        {
            bannerAds.HideBanner();
            Debug.Log("Game Over Banner hidden.");
        }
        else
        {
            Debug.LogWarning("BannerAdsManager instance not found.");
        }
    }
}