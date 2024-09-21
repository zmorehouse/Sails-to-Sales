using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void StartGame()
    {
        // Reset the score
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
        else
        {
            Debug.LogError("ScoreManager.Instance is null");
        }

        // Load the game scene
        SceneManager.LoadScene("Game"); // Replace "Game" with the actual name of your game scene
    }
}
