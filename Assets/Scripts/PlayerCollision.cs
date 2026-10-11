using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    public GameObject gameOverPanel;

    [Header("Continue Settings")]
    public float continueInvulnerabilityTime = 1.5f;

    private bool gameOver = false;
    private bool continueUsed = false;

    private Collider2D playerCollider;

    private void Start()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            // Ignore additional obstacle collisions after Game Over
            if (gameOver)
            {
                return;
            }

            gameOver = true;

            Debug.Log("GAME OVER!");

            // Play obstacle hit sound only if Sound is ON
            if (MainMenuSettings.IsSoundEnabled())
            {
                AudioSource playerAudio = GetComponent<AudioSource>();

                if (playerAudio != null && playerAudio.clip != null)
                {
                    playerAudio.Play();
                }
            }

            // Show Game Over panel
            gameOverPanel.SetActive(true);

            // Play Game Over sound only if Sound is ON
            if (MainMenuSettings.IsSoundEnabled())
            {
                AudioSource gameOverAudio =
                    gameOverPanel.GetComponent<AudioSource>();

                if (gameOverAudio != null && gameOverAudio.clip != null)
                {
                    gameOverAudio.Play();
                }
            }

            // Stop the score
            ScoreManager scoreManager =
                FindFirstObjectByType<ScoreManager>();

            if (scoreManager != null)
            {
                scoreManager.StopScoring();
            }
        }
    }

    // Called after the player successfully earns the rewarded ad
    public void ContinueGame()
    {
        // Continue can only be used once per run
        if (!gameOver || continueUsed)
        {
            Debug.LogWarning("Continue is not available.");
            return;
        }

        continueUsed = true;
        gameOver = false;

        // Hide Game Over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Resume score
        ScoreManager scoreManager =
            FindFirstObjectByType<ScoreManager>();

        if (scoreManager != null)
        {
            scoreManager.ResumeScoring();
        }

        // Brief invulnerability
        if (playerCollider != null)
        {
            StartCoroutine(ContinueInvulnerability());
        }

        Debug.Log("GAME CONTINUED AFTER REWARDED AD.");
    }

    private IEnumerator ContinueInvulnerability()
    {
        playerCollider.enabled = false;

        yield return new WaitForSeconds(
            continueInvulnerabilityTime
        );

        playerCollider.enabled = true;

        Debug.Log("Player invulnerability ended.");
    }
}