using UnityEngine;

public class ButtonClickAudio : MonoBehaviour
{
    [Header("Button Click Sound")]
    public AudioClip clickSound;

    [Range(0f, 1f)]
    public float volume = 0.8f;

    private AudioSource clickAudioSource;

    private void Awake()
    {
        // Create a separate Audio Source for button clicks.
        GameObject audioObject = new GameObject("ButtonClickAudioSource");

        audioObject.transform.SetParent(transform);

        clickAudioSource = audioObject.AddComponent<AudioSource>();

        clickAudioSource.playOnAwake = false;
        clickAudioSource.loop = false;
        clickAudioSource.volume = volume;
    }

    public void PlayClickSound()
    {
        // Respect the game's Sound setting.
        if (!MainMenuSettings.IsSoundEnabled())
        {
            return;
        }

        if (clickSound != null)
        {
            clickAudioSource.PlayOneShot(
                clickSound,
                volume
            );
        }
    }
}