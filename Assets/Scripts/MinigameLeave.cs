using UnityEngine;
using UnityEngine.SceneManagement;  // Needed to change the scene

public class PopupTrigger : MonoBehaviour
{
    public GameObject popupPanel;  // Assign this in the Inspector
    private bool isPlayerInZone = false;  // To track if the player is in the zone

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
        // If the player is in the zone and presses the 'E' key, change the scene
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            // Change to the desired scene
            SceneManager.LoadScene("game", LoadSceneMode.Single);
        }
    }
}
