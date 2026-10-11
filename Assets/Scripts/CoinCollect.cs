using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("COIN COLLECTED!");

            // Play coin collection sound only if Sound is ON
            if (MainMenuSettings.IsSoundEnabled())
            {
                AudioSource audioSource = GetComponent<AudioSource>();

                if (audioSource != null && audioSource.clip != null)
                {
                    AudioSource.PlayClipAtPoint(
                        audioSource.clip,
                        Camera.main.transform.position,
                        audioSource.volume
                    );
                }
            }

            // Add coin
            CoinManager coinManager =
                FindFirstObjectByType<CoinManager>();

            if (coinManager != null)
            {
                coinManager.AddCoin();
            }

            // Recycle this coin instead of destroying it
            CoinMovement coinMovement =
                GetComponent<CoinMovement>();

            if (coinMovement != null)
            {
                coinMovement.RecycleCoin();
            }
        }
    }
}