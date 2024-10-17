using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTutorial : MonoBehaviour
{
    public GameObject popupPanel;  // Assign this in the Inspector
    private bool isPlayerInZone = false;  // To track if the player is in the zone
    private GameObject[] sceneObjects;  // To hold the original scene objects
    private string minigameSceneName = "minigame";  // Name of the minigame scene
    private string gameSceneName = "Game";  // Name of the main game scene

    private void Start()
    {
        // Store the objects of the active scene to be destroyed later
        sceneObjects = SceneManager.GetActiveScene().GetRootGameObjects();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger zone is the player
        if (other.CompareTag("Player"))
        {
            // Show the popup when the player enters the zone
            popupPanel.SetActive(true);
            isPlayerInZone = true;  // Player is now in the zone
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger zone is the player
        if (other.CompareTag("Player"))
        {
            // Hide the popup when the player exits the zone
            popupPanel.SetActive(false);
            isPlayerInZone = false;  // Player left the zone
        }
    }

    private void Update()
    {
        // If the player is in the zone and presses the 'E' key, destroy everything and return to the main game scene
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            DestroyMinigameScene();
        }
    }

    // Method to destroy all scene objects and load the main game scene
    private void DestroyMinigameScene()
    {
        // Destroy all objects in the current scene
        foreach (GameObject obj in sceneObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }



        // Load the main game scene
        SceneManager.LoadScene(gameSceneName);
    }
}
