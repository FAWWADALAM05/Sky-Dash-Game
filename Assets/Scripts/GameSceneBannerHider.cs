using UnityEngine;

public class GameSceneBannerHider : MonoBehaviour
{
    private void Start()
    {
        BannerAdsManager bannerAds =
            FindFirstObjectByType<BannerAdsManager>();

        if (bannerAds != null)
        {
            bannerAds.HideBanner();
            Debug.Log("Game Scene: Banner hidden during active gameplay.");
        }
        else
        {
            Debug.LogWarning("BannerAdsManager not found.");
        }
    }
}