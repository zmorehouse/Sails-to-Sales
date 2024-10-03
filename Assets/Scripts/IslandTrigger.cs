// A script used to manage interactions with islands in the game.
using UnityEngine;

public class IslandTrigger : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private ShipController shipController; 
    private Renderer islandRenderer;
    private bool isUsed = false; 
    private Color originalColor; 
    private QuotaManager quotaManager; 

    private void Start()
    {
        // Find the player (ship) GameObject and get the ShipController component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            shipController = player.GetComponent<ShipController>();
        }

        // Get the Renderer component from the island to change its color/material
        islandRenderer = GetComponent<Renderer>();
        if (islandRenderer == null)
        {
            Debug.LogError("No Renderer found on the island object!");
        }
        else
        {
            originalColor = islandRenderer.material.color; // Store the original color of the island
        }

        // Find the QuotaManager in the scene
        quotaManager = FindObjectOfType<QuotaManager>();
    }

    private void Update()
    {
        // Check if the player is in range and presses the 'E' key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (shipController != null && shipController.CanInteractWithIsland() && !isUsed)
            {
                // This is where we'd implement scene switching logic

                shipController.DecreaseDayLimit(); 
                MakeIslandGrey();
                isUsed = true; 
                quotaManager.IncrementQuota(); 
            }
            else if (isUsed)
            {
                Debug.Log("This island has already been used this cycle.");
            }
            else
            {
                Debug.Log("Player has no days left and cannot interact with islands.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    // Method to make the island go grey by changing its color
    private void MakeIslandGrey()
    {
        if (islandRenderer != null)
        {
            islandRenderer.material.color = Color.grey; 
            Debug.Log("Island turned grey after interaction.");
        }
    }

    // Method to reset the island to its original color and state
    public void ResetIsland()
    {
        if (islandRenderer != null)
        {
            islandRenderer.material.color = originalColor; 
        }
        isUsed = false; // Mark the island as not used
        Debug.Log("Island has been reset to its original state.");
    }
}
