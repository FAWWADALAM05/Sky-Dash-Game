using UnityEngine;
using TMPro;

public class MainMenuBestScore : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        bestScoreText.text = "BEST: " + highScore;
    }
}