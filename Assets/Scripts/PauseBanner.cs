using UnityEngine;

public class PauseBanner : MonoBehaviour
{
    private void OnEnable()
    {
        BannerAdsManager bannerAds =
            BannerAdsManager.Instance;

        if (bannerAds != null)
        {
            bannerAds.ShowBanner();
            Debug.Log("Pause Banner shown.");
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
            Debug.Log("Pause Banner hidden.");
        }
        else
        {
            Debug.LogWarning("BannerAdsManager instance not found.");
        }
    }
}