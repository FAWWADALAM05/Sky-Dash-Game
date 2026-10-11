using UnityEngine;
using TMPro;

public class MainMenuCoins : MonoBehaviour
{
    public TextMeshProUGUI totalCoinsText;

    private void Start()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        totalCoinsText.text = "COINS: " + totalCoins;
    }
}