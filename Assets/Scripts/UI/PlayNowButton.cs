using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Import for UI elements


public class SceneSwitcher : MonoBehaviour
{
    public Toggle tutorialToggle; // Reference to the toggle checkbox

    public void StartGame()
    {
        // Reset the score when the game is restarted
        if (ScoreManager.Instance != null)
        {
            
            ScoreManager.Instance.ResetScore();  // Ensure score is reset to 0
        }

        // Check if the tutorial toggle is on, load the appropriate scene
        if (tutorialToggle != null && tutorialToggle.isOn)
        {
            SceneManager.LoadScene("Tutorial"); // Load the tutorial scene if the checkbox is checked
        }
        else
        {
            // Load the game scene and enable ship movement
            SceneManager.sceneLoaded += OnGameSceneLoaded; // Register callback
            SceneManager.LoadScene("Game"); // Otherwise, load the main game scene
        }
    }

    // This method will be called after the game scene is loaded
    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Unregister the callback to avoid repeating it
        SceneManager.sceneLoaded -= OnGameSceneLoaded;

        // Find the ship object by its tag "Player"
        GameObject ship = GameObject.FindGameObjectWithTag("Player");

        if (ship != null)
        {
            // Get the ShipController component
            ShipController shipController = ship.GetComponent<ShipController>();
            
            if (shipController != null)
            {
                // Set canMove to true so the player can immediately control the ship
                shipController.SetMovement(true);
            }
        }
    }
}
