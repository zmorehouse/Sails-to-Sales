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
            
            // Update the high score if the current score is higher
            ScoreManager.Instance.CheckAndUpdateHighScore();
            
            // Get the updated high score
            int highScore = ScoreManager.Instance.GetHighScore();
            
            scoreText.text = "You completed " + score + " deliveries!";
            highScoreText.text = "High Score: " + highScore;
        }
    }
}
