using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSkinManager : MonoBehaviour
{
    [Header("Preview")]
    public GameObject[] skinPreviews;

    [Header("Shop Information")]
    public TMP_Text playerName;
    public TMP_Text priceText;

    [Header("Buttons")]
    public Button previousButton;
    public Button nextButton;
    public Button buyButton;

    [Header("Button Text")]
    public TMP_Text buyButtonText;

    [Header("Button Images")]
    public Sprite buyImage;
    public Sprite selectImage;
    public Sprite selectedImage;

    private int currentIndex = 0;

    private string[] skinNames =
    {
        "SKY ROOKIE",
        "BLUE BOLT",
        "GREEN COMET",
        "FIRE FLYER",
        "THUNDER JET",
        "GOLDEN EAGLE",
        "CLOUD RACER",
        "SUNSET JET",
        "CRYSTAL SKY",
        "SKY MASTER"
    };

    private int[] prices =
    {
        0,      // Sky Rookie
        250,    // Blue Bolt
        500,    // Green Comet
        750,    // Fire Flyer
        1000,   // Thunder Jet
        1500,   // Golden Eagle
        2500,   // Cloud Racer
        5000,   // Sunset Jet
        7000,     // Crystal Sky - TEST PRICE
        10000    // Sky Master
    };

    private void Start()
    {
        // Sky Rookie is always owned
        PlayerPrefs.SetInt("SkinOwned_0", 1);

        // Sky Rookie is the default equipped skin
        if (!PlayerPrefs.HasKey("EquippedSkin"))
        {
            PlayerPrefs.SetInt("EquippedSkin", 0);
        }

        PlayerPrefs.Save();

        // TEMPORARY:
        // Reset Crystal Sky ownership for testing.
        // Remove this line after testing Crystal Sky.
        ResetCrystalSkyForTesting();

        ShowSkin(0);
    }

    public void NextSkin()
    {
        currentIndex++;

        if (currentIndex >= skinPreviews.Length)
        {
            currentIndex = 0;
        }

        ShowSkin(currentIndex);
    }

    public void PreviousSkin()
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = skinPreviews.Length - 1;
        }

        ShowSkin(currentIndex);
    }

    private void ShowSkin(int index)
    {
        currentIndex = index;

        // Show only the current airplane preview
        for (int i = 0; i < skinPreviews.Length; i++)
        {
            if (skinPreviews[i] != null)
            {
                skinPreviews[i].SetActive(i == index);
            }
        }

        // Player name
        if (playerName != null && index < skinNames.Length)
        {
            playerName.text = skinNames[index];
        }

        // Price
        if (priceText != null)
        {
            // Sky Rookie is free
            // Purchased skins do not show price
            if (index == 0 || IsSkinOwned(index))
            {
                priceText.text = "";
            }
            else
            {
                priceText.text = prices[index].ToString();
            }
        }

        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (buyButton == null)
        {
            return;
        }

        Image buttonImage = buyButton.GetComponent<Image>();

        bool owned = IsSkinOwned(currentIndex);
        bool equipped = IsSkinEquipped(currentIndex);

        // Disable Unity's automatic faded disabled-button effect.
        buyButton.transition = Selectable.Transition.None;

        if (buttonImage != null)
        {
            buttonImage.color = Color.white;
        }

        // --------------------------------
        // NOT OWNED → BUY
        // --------------------------------

        if (!owned)
        {
            if (buttonImage != null && buyImage != null)
            {
                buttonImage.sprite = buyImage;
                buttonImage.color = Color.white;
            }

            buyButton.interactable = true;

            Debug.Log(
                "Button State: BUY - " +
                skinNames[currentIndex] +
                " - Price: " +
                prices[currentIndex]
            );
        }

        // --------------------------------
        // OWNED + EQUIPPED → SELECTED
        // --------------------------------

        else if (equipped)
        {
            if (buttonImage != null && selectedImage != null)
            {
                buttonImage.sprite = selectedImage;
                buttonImage.color = Color.white;
            }

            buyButton.interactable = false;

            Debug.Log(
                "Button State: SELECTED - " +
                skinNames[currentIndex]
            );
        }

        // --------------------------------
        // OWNED + NOT EQUIPPED → SELECT
        // --------------------------------

        else
        {
            if (buttonImage != null && selectImage != null)
            {
                buttonImage.sprite = selectImage;
                buttonImage.color = Color.white;
            }

            buyButton.interactable = true;

            Debug.Log(
                "Button State: SELECT - " +
                skinNames[currentIndex]
            );
        }

        // Text is not used anymore
        if (buyButtonText != null)
        {
            buyButtonText.text = "";
        }
    }

    public void BuyCurrentSkin()
    {
        // --------------------------------
        // ALREADY OWNED → SELECT
        // --------------------------------

        if (IsSkinOwned(currentIndex))
        {
            EquipCurrentSkin();
            return;
        }

        int price = prices[currentIndex];

        // Check Shop Currency Manager
        if (ShopCurrencyManager.Instance == null)
        {
            Debug.LogError(
                "ShopCurrencyManager not found!"
            );

            return;
        }

        // Check coins
        if (!ShopCurrencyManager.Instance.CanAfford(price))
        {
            Debug.Log(
                "NOT ENOUGH COINS! " +
                skinNames[currentIndex] +
                " costs " +
                price +
                ", but you only have " +
                ShopCurrencyManager.Instance.coins
            );

            return;
        }

        // Spend coins
        ShopCurrencyManager.Instance.SpendCoins(price);

        // Save ownership
        PlayerPrefs.SetInt(
            "SkinOwned_" + currentIndex,
            1
        );

        PlayerPrefs.Save();

        Debug.Log(
            "Skin purchased and unlocked: " +
            skinNames[currentIndex] +
            " for " +
            price +
            " coins."
        );

        // BUY → SELECT
        UpdateButtonState();

        // Remove price after purchase
        if (priceText != null)
        {
            priceText.text = "";
        }
    }

    public void EquipCurrentSkin()
    {
        // Make sure the skin is owned
        if (!IsSkinOwned(currentIndex))
        {
            Debug.Log(
                "Skin is not owned: " +
                skinNames[currentIndex]
            );

            return;
        }

        // Save equipped skin
        PlayerPrefs.SetInt(
            "EquippedSkin",
            currentIndex
        );

        PlayerPrefs.Save();

        Debug.Log(
            "Skin selected: " +
            skinNames[currentIndex]
        );

        // SELECT → SELECTED
        UpdateButtonState();

        // Hide price
        if (priceText != null)
        {
            priceText.text = "";
        }
    }

    private bool IsSkinOwned(int index)
    {
        return PlayerPrefs.GetInt(
            "SkinOwned_" + index,
            0
        ) == 1;
    }

    private bool IsSkinEquipped(int index)
    {
        return PlayerPrefs.GetInt(
            "EquippedSkin",
            0
        ) == index;
    }

    // --------------------------------
    // TEMPORARY CRYSTAL SKY TEST RESET
    // --------------------------------

    public void ResetCrystalSkyForTesting()
    {
        // Crystal Sky = index 8
        PlayerPrefs.DeleteKey("SkinOwned_8");

        PlayerPrefs.Save();

        Debug.Log(
            "Crystal Sky ownership reset for testing."
        );
    }
}