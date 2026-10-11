using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;

    [Header("Button Sound")]
    public AudioClip buttonClickSound;

    [Header("Pause Sound")]
    public AudioClip pauseSound;

    public void PauseGame()
    {
        // Play button click sound
        PlayButtonSound();

        // Play pause sound
        if (pauseSound != null && MainMenuSettings.IsSoundEnabled())
        {
            AudioSource.PlayClipAtPoint(
                pauseSound,
                Camera.main.transform.position
            );
        }

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        // Play button click sound after game resumes
        PlayButtonSound();
    }

    private void PlayButtonSound()
    {
        if (buttonClickSound != null &&
            MainMenuSettings.IsSoundEnabled())
        {
            AudioSource.PlayClipAtPoint(
                buttonClickSound,
                Camera.main.transform.position
            );
        }
    }
}