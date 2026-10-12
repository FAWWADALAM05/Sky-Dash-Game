using UnityEngine;

public class SettingsBanner : MonoBehaviour
{
    private void OnEnable()
    {
        BannerAdsManager bannerAds =
            FindFirstObjectByType<BannerAdsManager>();

        if (bannerAds != null)
        {
            bannerAds.ShowBanner();

            Debug.Log(
                "Settings Banner requested."
            );
        }
        else
        {
            Debug.LogWarning(
                "BannerAdsManager not found."
            );
        }
    }
}