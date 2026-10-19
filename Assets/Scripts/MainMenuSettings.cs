using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettings : MonoBehaviour
{
    [Header("Main Menu Music")]
    public AudioSource mainMenuMusic;

    [Header("Settings Labels")]
    public Text musicLabel;
    public Text soundLabel;

    private void Start()
    {
        // Load saved Music setting
        bool musicEnabled =
            PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

        ApplyMusicSetting(musicEnabled);

        // Load saved Sound setting
        bool soundEnabled =
            PlayerPrefs.GetInt("SoundEnabled", 1) == 1;

        ApplySoundSetting(soundEnabled);
    }

    // =========================================================
    // MUSIC
    // =========================================================

    public void SetMusic(bool isOn)
    {
        // Save Music setting
        PlayerPrefs.SetInt(
            "MusicEnabled",
            isOn ? 1 : 0
        );

        PlayerPrefs.Save();

        ApplyMusicSetting(isOn);
    }

    private void ApplyMusicSetting(bool isOn)
    {
        if (mainMenuMusic != null)
        {
            // Music ON/OFF
            mainMenuMusic.mute = !isOn;

            if (isOn)
            {
                if (!mainMenuMusic.isPlaying)
                {
                    mainMenuMusic.Play();
                }
            }
            else
            {
                // Stop current music when OFF
                if (mainMenuMusic.isPlaying)
                {
                    mainMenuMusic.Stop();
                }
            }
        }

        // Update Music ON/OFF text
        if (musicLabel != null)
        {
            musicLabel.text = isOn ? "ON" : "OFF";
        }
    }

    // Allows other scripts to check Music status
    public static bool IsMusicEnabled()
    {
        return PlayerPrefs.GetInt(
            "MusicEnabled",
            1
        ) == 1;
    }

    // =========================================================
    // SOUND
    // =========================================================

    public void SetSound(bool isOn)
    {
        // Save Sound setting
        PlayerPrefs.SetInt(
            "SoundEnabled",
            isOn ? 1 : 0
        );

        PlayerPrefs.Save();

        ApplySoundSetting(isOn);
    }

    private void ApplySoundSetting(bool isOn)
    {
        // Update Sound ON/OFF text
        if (soundLabel != null)
        {
            soundLabel.text = isOn ? "ON" : "OFF";
        }
    }

    // Allows other scripts to check Sound status
    public static bool IsSoundEnabled()
    {
        return PlayerPrefs.GetInt(
            "SoundEnabled",
            1
        ) == 1;
    }
}