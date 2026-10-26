using UnityEngine;
using TMPro;

public class ShopCurrencyManager : MonoBehaviour
{
    public static ShopCurrencyManager Instance;

    [Header("Coins")]
    public int coins;

    [Header("UI")]
    public TMP_Text coinText;

    private const string COIN_KEY = "TotalCoins";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Load the REAL saved coin balance
        coins = PlayerPrefs.GetInt(COIN_KEY, 0);

        UpdateCoinText();
    }

    public bool CanAfford(int amount)
    {
        return coins >= amount;
    }

    public void SpendCoins(int amount)
    {
        if (!CanAfford(amount))
            return;

        coins -= amount;

        // Save the same coin balance used by Main Menu
        PlayerPrefs.SetInt(COIN_KEY, coins);
        PlayerPrefs.Save();

        UpdateCoinText();
    }

    public void AddCoins(int amount)
    {
        coins += amount;

        // Save the same coin balance used by Main Menu
        PlayerPrefs.SetInt(COIN_KEY, coins);
        PlayerPrefs.Save();

        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = coins.ToString();
        }
    }
}