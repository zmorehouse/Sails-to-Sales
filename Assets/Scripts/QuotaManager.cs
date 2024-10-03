// A script used to manage the quota of islands the player must visit per cycle.
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using UnityEngine.SceneManagement;

public class QuotaManager : MonoBehaviour
{
    public int requiredQuota = 3; // The number of islands the player must visit per cycle
    public int currentQuota = 0;  // The current number of islands visited by the player

    public TextMeshProUGUI quotaText;

    private void Start()
    {
        // Update the UI at the start to show the initial quota
        UpdateQuotaUI();
    }

    // Method to increment the quota when the player interacts with an island
    public void IncrementQuota()
    {
        currentQuota++;
        Debug.Log($"Quota incremented. Current quota: {currentQuota}/{requiredQuota}");
        UpdateQuotaUI(); 
    }

    // Method to check if the quota has been met
    public bool HasMetQuota()
    {
        return currentQuota >= requiredQuota;
    }

    // Method to reset the quota at the start of a new cycle
    public void ResetQuota()
    {
        currentQuota = 0;
        Debug.Log("Quota has been reset for the new day cycle.");
        UpdateQuotaUI(); 
    }

    // Method to update the quota text UI
    private void UpdateQuotaUI()
    {
        if (quotaText != null)
        {
            quotaText.text = $"Quota: {currentQuota} / {requiredQuota}";
        }
    }

    // Method to handle the player losing the game
    public void LoseGame()
    {
        Debug.Log("Player has failed to meet the quota. Game Over.");
        SceneManager.LoadScene("GameOver"); 
    }
}
