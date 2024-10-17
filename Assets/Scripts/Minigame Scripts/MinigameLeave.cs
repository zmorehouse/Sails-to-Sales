using UnityEngine;
using UnityEngine.SceneManagement;  // Needed to manage the scene

public class PopupTrigger : MonoBehaviour
{
    public GameObject popupPanel;  // Assign this in the Inspector
    private bool isPlayerInZone = false;  // To track if the player is in the zone
    private GameObject[] sceneObjects;  // To hold the original scene objects
    private string minigameSceneName = "minigame";  // Name of the minigame scene

    private void Start()
    {
        // Store the objects of the active scene to be disabled/re-enabled
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
        // Hide the popup when the player exits the zone
        if (other.CompareTag("Player"))
        {
            popupPanel.SetActive(false);
            isPlayerInZone = false;  // Player left the zone
        }
    }

    private void Update()
    {
        // If the player is in the zone and presses the 'E' key, return to the previous scene
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            // Disable the popup panel
            popupPanel.SetActive(false);

            // Transfer the current resources to total
            ResourceManager.instance.TransferCurrentResourcesToTotal();  // Transfer resources

            // Destroy the minigame objects and unload the scene
            DestroyMinigameScene();
        }
    }

    public void DestroyMinigameScene()
    {
        // Unload the minigame scene
        if (SceneManager.GetSceneByName(minigameSceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(minigameSceneName).completed += (operation) =>
            {
                // Re-enable all objects in the original scene once the minigame is unloaded
                EnablePreviousSceneObjects();
            };
        }
        else
        {
            Debug.LogError("Minigame scene is not loaded!");
        }
    }

    private void EnablePreviousSceneObjects()
    {
        // Re-enable all objects in the original scene
        foreach (GameObject obj in sceneObjects)
        {
            // Find the shop menu dynamically by its tag or name and skip re-enabling it
            if (obj.CompareTag("ShopMenu"))  // Assuming the shop menu has the tag "ShopMenu"
            {
                continue;  // Skip re-enabling the shop menu
            }

            obj.SetActive(true);  // Re-enable each object
        }

        Debug.Log("Previous scene objects re-enabled, shop menu skipped.");
    }
}
