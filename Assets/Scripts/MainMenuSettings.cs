using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettings : MonoBehaviour
{
    public AudioSource mainMenuMusic;

    public Text musicLabel;
    public Text soundLabel;

    public void SetMusic(bool isOn)
    {
        // Save Music setting
        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();

        if (mainMenuMusic != null)
        {
            mainMenuMusic.mute = !isOn;

            if (isOn && !mainMenuMusic.isPlaying)
            {
                mainMenuMusic.Play();
            }
        }

        // Update Music ON/OFF text
        if (musicLabel != null)
        {
            musicLabel.text = isOn ? "ON" : "OFF";
        }
    }

    public void SetSound(bool isOn)
    {
        PlayerPrefs.SetInt("SoundEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();

        // Update Sound ON/OFF text
        if (soundLabel != null)
        {
            soundLabel.text = isOn ? "ON" : "OFF";
        }
    }

    public static bool IsSoundEnabled()
    {
        return PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
    }
}