using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("Pause Panel")]
    public GameObject pausePanel;

    [Header("Player")]
    public Animator playerAnimator;

    public void PauseGame()
    {
        // Show Pause Panel
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        // Stop player animation
        if (playerAnimator != null)
        {
            playerAnimator.speed = 0f;
        }

        // Stop all gameplay movement and physics
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        // Resume gameplay
        Time.timeScale = 1f;

        // Resume player animation
        if (playerAnimator != null)
        {
            playerAnimator.speed = 1f;
        }

        // Hide Pause Panel
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // Safety: restore normal time
        Time.timeScale = 1f;

        if (playerAnimator != null)
        {
            playerAnimator.speed = 1f;
        }
    }
}