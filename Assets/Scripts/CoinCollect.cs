using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    private CoinMovement coinMovement;

    private void Start()
    {
        coinMovement = GetComponent<CoinMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("COIN COLLECTED!");

            if (MainMenuSettings.IsSoundEnabled())
            {
                AudioSource audioSource =
                    GetComponent<AudioSource>();

                if (audioSource != null &&
                    audioSource.clip != null)
                {
                    AudioSource.PlayClipAtPoint(
                        audioSource.clip,
                        Camera.main.transform.position,
                        audioSource.volume
                    );
                }
            }

            CoinManager coinManager =
                FindFirstObjectByType<CoinManager>();

            if (coinManager != null)
            {
                coinManager.AddCoin();
            }

            if (coinMovement != null)
            {
                coinMovement.RecycleCoin();
            }
        }
    }
}