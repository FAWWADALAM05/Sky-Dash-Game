using UnityEngine;
using GoogleMobileAds.Ump.Api;

public class ConsentManager : MonoBehaviour
{
    public static bool AdsCanBeRequested { get; private set; }

    private const string PrivacyPolicyURL =
        "https://fawwadalam05.github.io/Sky-Dash/";

    private void Start()
    {
        UpdateConsent();
    }

    private void UpdateConsent()
    {
        ConsentRequestParameters requestParameters =
            new ConsentRequestParameters();

        ConsentInformation.Update(
            requestParameters,
            OnConsentInfoUpdated
        );
    }

    private void OnConsentInfoUpdated(FormError error)
    {
        if (error != null)
        {
            Debug.LogError(
                "Consent information update failed: " +
                error.Message
            );

            return;
        }

        Debug.Log(
            "Consent information updated."
        );

        ConsentForm.LoadAndShowConsentFormIfRequired(
            (FormError formError) =>
            {
                if (formError != null)
                {
                    Debug.LogError(
                        "Consent form error: " +
                        formError.Message
                    );

                    return;
                }

                Debug.Log(
                    "Consent form process completed."
                );

                CheckCanRequestAds();
            }
        );
    }

    private void CheckCanRequestAds()
    {
        if (ConsentInformation.CanRequestAds())
        {
            AdsCanBeRequested = true;

            Debug.Log(
                "CONSENT OK - ADS CAN BE REQUESTED."
            );
        }
        else
        {
            Debug.Log(
                "CONSENT NOT AVAILABLE - ADS CANNOT BE REQUESTED."
            );
        }
    }

    public void OpenPrivacyPolicy()
    {
        Debug.Log(
            "Opening Sky Dash Privacy Policy."
        );

        Application.OpenURL(
            PrivacyPolicyURL
        );
    }
}