using UnityEngine;
using CrazyGames;

public class CrazyGamesGameplay : MonoBehaviour
{
    private void Start()
    {
    }

    public void StartGameplay()
    {
        if (!CrazySDK.IsAvailable)
        {
            Debug.Log("CrazyGames SDK is not available.");
            return;
        }

        if (!CrazySDK.IsInitialized)
        {
            Debug.Log("CrazyGames SDK is not initialized yet.");
            return;
        }

        CrazySDK.Game.GameplayStart();

        Debug.Log("CRAZYGAMES GAMEPLAY START");
    }

    public void StopGameplay()
    {
        if (!CrazySDK.IsAvailable)
        {
            return;
        }

        if (!CrazySDK.IsInitialized)
        {
            return;
        }

        CrazySDK.Game.GameplayStop();

        Debug.Log("CRAZYGAMES GAMEPLAY STOP");
    }
}