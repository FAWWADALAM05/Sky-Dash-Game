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

    private PlayerFlight playerFlight;
    private Animator playerAnimator;

    private void Start()
    {
        // Make sure normal game time is active when the game starts.
        Time.timeScale = 1f;

        // Get Player components.
        playerCollider = GetComponent<Collider2D>();
        playerFlight = GetComponent<PlayerFlight>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            // Ignore additional obstacle collisions after Game Over.
            if (gameOver)
            {
                return;
            }

            gameOver = true;

            Debug.Log("GAME OVER!");

            // ---------------------------------------------
            // PLAY PLAYER GAME OVER ANIMATION
            // ---------------------------------------------
            if (playerAnimator != null)
            {
                // Make sure the Animator uses unscaled time.
                playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

                // Clear any previous GameOver trigger.
                playerAnimator.ResetTrigger("GameOver");

                // Directly play the GameOver animation from the beginning.
                playerAnimator.Play("GameOver", 0, 0f);

                Debug.Log("GAME OVER ANIMATION PLAYING.");
            }
            else
            {
                Debug.LogWarning("Player Animator not found.");
            }

            // ---------------------------------------------
            // STOP PLAYER FLIGHT
            // ---------------------------------------------
            if (playerFlight != null)
            {
                playerFlight.StopFlight();

                Debug.Log("PLAYER FLIGHT STOPPED.");
            }

            // ---------------------------------------------
            // PLAY OBSTACLE HIT SOUND
            // ---------------------------------------------
            if (MainMenuSettings.IsSoundEnabled())
            {
                AudioSource playerAudio =
                    GetComponent<AudioSource>();

                if (playerAudio != null &&
                    playerAudio.clip != null)
                {
                    playerAudio.Play();
                }
            }

            // ---------------------------------------------
            // SHOW GAME OVER PANEL
            // ---------------------------------------------
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            // ---------------------------------------------
            // PLAY GAME OVER SOUND
            // ---------------------------------------------
            if (MainMenuSettings.IsSoundEnabled())
            {
                if (gameOverPanel != null)
                {
                    AudioSource gameOverAudio =
                        gameOverPanel.GetComponent<AudioSource>();

                    if (gameOverAudio != null &&
                        gameOverAudio.clip != null)
                    {
                        gameOverAudio.Play();
                    }
                }
            }

            // ---------------------------------------------
            // STOP SCORE
            // ---------------------------------------------
            ScoreManager scoreManager =
                FindFirstObjectByType<ScoreManager>();

            if (scoreManager != null)
            {
                scoreManager.StopScoring();
            }

            // ---------------------------------------------
            // FREEZE ENTIRE GAMEPLAY WORLD
            // ---------------------------------------------
            //
            // This freezes:
            // Player
            // Clouds
            // Floating Islands
            // Obstacles
            // Coins
            // Physics
            // Other normal gameplay movement
            //
            Time.timeScale = 0f;

            Debug.Log("ENTIRE GAMEPLAY WORLD FROZEN.");
        }
    }

    // Called after the player successfully earns the rewarded ad.
    public void ContinueGame()
    {
        // Continue can only be used once per run.
        if (!gameOver || continueUsed)
        {
            Debug.LogWarning("Continue is not available.");
            return;
        }

        continueUsed = true;
        gameOver = false;

        // ---------------------------------------------
        // RESUME ENTIRE GAMEPLAY WORLD
        // ---------------------------------------------
        Time.timeScale = 1f;

        Debug.Log("GAMEPLAY WORLD RESUMED.");

        // ---------------------------------------------
        // RESUME PLAYER FLIGHT
        // ---------------------------------------------
        if (playerFlight != null)
        {
            playerFlight.ResumeFlight();

            Debug.Log("PLAYER FLIGHT RESUMED.");
        }

        // ---------------------------------------------
        // HIDE GAME OVER PANEL
        // ---------------------------------------------
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // ---------------------------------------------
        // RESUME SCORE
        // ---------------------------------------------
        ScoreManager scoreManager =
            FindFirstObjectByType<ScoreManager>();

        if (scoreManager != null)
        {
            scoreManager.ResumeScoring();
        }

        // ---------------------------------------------
        // TEMPORARY INVULNERABILITY
        // ---------------------------------------------
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

        Debug.Log("PLAYER INVULNERABILITY ENDED.");
    }

    private void OnDestroy()
    {
        // Safety reset.
        // Prevent the next scene from remaining frozen.
        Time.timeScale = 1f;
    }
}