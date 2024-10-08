using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Import for UI elements

public class SceneSwitcher : MonoBehaviour
{
    public Toggle tutorialToggle; // Reference to the toggle checkbox

    public void StartGame()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore(); // Reset the score when the game starts
        }

        // Check if the tutorial toggle is on, load the appropriate scene
        if (tutorialToggle != null && tutorialToggle.isOn)
        {
            SceneManager.LoadScene("Tutorial"); // Load the tutorial scene if the checkbox is checked
        }
        else
        {
            SceneManager.LoadScene("Game"); // Otherwise, load the main game scene
        }
    }
}
