// A script used to manage high scores and scores when game over condition is satisfied. 

using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        if (ScoreManager.Instance != null)
        {
            int score = ScoreManager.Instance.GetScore();
            ScoreManager.Instance.CheckAndUpdateHighScore();
            int highScore = ScoreManager.Instance.GetHighScore();
            scoreText.text = "You completed " + score + " deliveries!";
            highScoreText.text = "High Score: " + highScore;
        }
    }
}
