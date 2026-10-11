using UnityEngine;

public class GameMusicSettings : MonoBehaviour
{
    private AudioSource musicSource;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

        if (musicSource != null)
        {
            musicSource.mute = !musicEnabled;

            if (musicEnabled && !musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
    }
}