using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;  // Singleton instance

    // Track resources from previous islands
    public int totalCoconuts = 0;
    public int totalMangoes = 0;
    public int totalBananas = 0;

    // Track resources from the current island
    public int currentCoconuts = 0;
    public int currentMangoes = 0;
    public int currentBananas = 0;

    public TextMeshProUGUI resourceText;     // UI element to display resources

    private void Awake()
    {
        // Singleton pattern to persist resources across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent the object from being destroyed on scene load
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Start()
    {
        // Update UI with the current resource count when the game starts
        UpdateResourceUI();
    }

    // Method to add coconuts on the current island
    public void AddCurrentCoconuts(int amount)
    {
        currentCoconuts += amount;
        Debug.Log($"Coconuts collected on this island: {currentCoconuts}");
        UpdateResourceUI();
    }

    // Method to add mangoes on the current island
    public void AddCurrentMangoes(int amount)
    {
        currentMangoes += amount;
        Debug.Log($"Mangoes collected on this island: {currentMangoes}");
        UpdateResourceUI();
    }

    // Method to add bananas on the current island
    public void AddCurrentBananas(int amount)
    {
        currentBananas += amount;
        Debug.Log($"Bananas collected on this island: {currentBananas}");
        UpdateResourceUI();
    }

    // Method to transfer current island resources to total after successful turn-in
    public void TransferCurrentResourcesToTotal()
    {
        totalCoconuts += currentCoconuts;
        totalMangoes += currentMangoes;
        totalBananas += currentBananas;

        // Reset current island resources
        ResetCurrentResources();

        Debug.Log("Transferred current island resources to total.");
        UpdateResourceUI();
    }

    // Method to reset current island resources (when hit by enemy or leaving island)
    public void ResetCurrentResources()
    {
        currentCoconuts = 0;
        currentMangoes = 0;
        currentBananas = 0;
        UpdateResourceUI();
    }

    // Method to update the resource UI
    private void UpdateResourceUI()
    {
        if (resourceText != null)
        {
            resourceText.text = $"Coconuts: {totalCoconuts + currentCoconuts} | Mangoes: {totalMangoes + currentMangoes} | Bananas: {totalBananas + currentBananas}";
        }
    }

    // Method to reset all resources (if needed)
    public void ResetAllResources()
    {
        totalCoconuts = 0;
        totalMangoes = 0;
        totalBananas = 0;
        ResetCurrentResources();  // Also reset current resources
        UpdateResourceUI();
    }
}
