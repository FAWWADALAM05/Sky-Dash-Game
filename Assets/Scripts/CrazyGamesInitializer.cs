using UnityEngine;
using CrazyGames;

public class CrazyGamesInitializer : MonoBehaviour
{
    private void Awake()
    {
        if (!CrazySDK.IsAvailable)
        {
            Debug.Log("CrazyGames SDK is not available.");
            return;
        }

        if (CrazySDK.IsInitialized)
        {
            Debug.Log("CrazyGames SDK is already initialized.");
            StartGameplay();
            return;
        }

        Debug.Log("Initializing CrazyGames SDK...");

        CrazySDK.Init(OnSDKInitialized);
    }

    private void OnSDKInitialized()
    {
        Debug.Log("CrazyGames SDK initialized successfully.");

        StartGameplay();
    }

    private void StartGameplay()
    {
        CrazyGamesGameplay gameplay = FindFirstObjectByType<CrazyGamesGameplay>();

        if (gameplay != null)
        {
            gameplay.StartGameplay();
        }
        else
        {
            Debug.LogWarning("CrazyGamesGameplay not found.");
        }
    }
}