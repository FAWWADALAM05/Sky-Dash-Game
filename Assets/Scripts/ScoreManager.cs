using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    [Header("Game Over")]
    public GameObject gameOverPanel;

    [Header("Score Settings")]
    public float scoreInterval = 1f;

    private int score = 0;
    private int highScore = 0;
    private float scoreTimer = 0f;

    private bool gameOver = false;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        UpdateScoreText();
        UpdateHighScoreText();
    }

    private void Update()
    {
        // Stop score completely after Game Over
        if (gameOver)
        {
            return;
        }

        // Extra safety check
        if (gameOverPanel != null && gameOverPanel.activeSelf)
        {
            return;
        }

        scoreTimer += Time.deltaTime;

        if (scoreTimer >= scoreInterval)
        {
            score++;
            scoreTimer = 0f;

            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
            }

            UpdateScoreText();
            UpdateHighScoreText();
        }
    }

    public void StopScoring()
    {
        gameOver = true;
        scoreTimer = 0f;

        Debug.Log("SCORE STOPPED - GAME OVER");
    }

    // Resume scoring after a rewarded ad Continue
    public void ResumeScoring()
    {
        gameOver = false;
        scoreTimer = 0f;

        Debug.Log("SCORE RESUMED - CONTINUE");
    }

    private void UpdateScoreText()
    {
        scoreText.text = "SCORE: " + score;
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "BEST: " + highScore;
    }
}