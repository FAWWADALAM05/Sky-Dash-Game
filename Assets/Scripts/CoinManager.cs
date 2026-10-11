using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    private int coins = 0;

    private void Start()
    {
        // Load saved coins
        coins = PlayerPrefs.GetInt("TotalCoins", 0);

        UpdateCoinText();
    }

    public void AddCoin()
    {
        coins++;

        // Save coins
        PlayerPrefs.SetInt("TotalCoins", coins);
        PlayerPrefs.Save();

        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        coinText.text = "COINS: " + coins;
    }
}