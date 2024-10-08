// A script used to manage the island limit and resource quota per cycle.
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using UnityEngine.SceneManagement;

public class QuotaManager : MonoBehaviour
{
    public int requiredIslandLimit = 3; // The number of islands the player must visit per cycle
    public int currentIslandVisits = 0;  // The current number of islands visited by the player

    public int currentResourceQuota = 1; // The number of resources the player needs to collect per cycle (starts at 1)
    public int resourceQuotaMultiplier = 1; // Multiplier to scale the resource quota as the player hits their target

    public TextMeshProUGUI islandLimitText; // UI element for displaying the island limit
    public TextMeshProUGUI resourceQuotaText; // UI element for displaying the resource quota

    private void Start()
    {
        // Update the UI at the start to show the initial island limit and resource quota
        UpdateIslandLimitUI();
        UpdateResourceQuotaUI();
    }

    // Method to increment the island visit count when the player interacts with an island
    public void IncrementIslandVisit()
    {
        currentIslandVisits++;
        Debug.Log($"Island visit incremented. Current visits: {currentIslandVisits}/{requiredIslandLimit}");
        UpdateIslandLimitUI(); 
    }

    // Method to check if the island limit has been met
    public bool HasMetIslandLimit()
    {
        return currentIslandVisits >= requiredIslandLimit;
    }

    // Method to reset the island visits and generate a new resource quota at the start of a new cycle
    public void ResetIslandLimit()
    {
        currentIslandVisits = 0;
        Debug.Log("Island limit has been reset for the new cycle.");
        
        // Generate a new resource quota and scale it
        currentResourceQuota = 1 * resourceQuotaMultiplier;
        resourceQuotaMultiplier++; // Increase the multiplier to scale the resource quota for future cycles
        Debug.Log($"New resource quota generated: {currentResourceQuota}");

        // Update the UI
        UpdateIslandLimitUI(); 
        UpdateResourceQuotaUI();
    }

    // Method to update the island limit text UI
    private void UpdateIslandLimitUI()
    {
        if (islandLimitText != null)
        {
            islandLimitText.text = $"Island Limit: {currentIslandVisits} / {requiredIslandLimit}";
        }
    }

    // Method to update the resource quota text UI
    private void UpdateResourceQuotaUI()
    {
        if (resourceQuotaText != null)
        {
            resourceQuotaText.text = $"Resource Quota: {currentResourceQuota}";
        }
    }

    // Method to handle the player losing the game (if they fail to meet the resource quota or island limit)
    public void LoseGame()
    {
        Debug.Log("Player has failed to meet the quota. Game Over.");
        SceneManager.LoadScene("GameOver"); 
    }
}
