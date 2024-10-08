using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;  // Singleton instance
    public int resources = 0;                // Player's resources
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

    // Method to add resources
    public void AddResources(int amount)
    {
        resources += amount;
        Debug.Log($"Resources collected: {resources}");
        UpdateResourceUI();
    }

    // Method to update the resource UI
    private void UpdateResourceUI()
    {
        if (resourceText != null)
        {
            resourceText.text = $"Resources: {resources}";
        }
    }

    // Method to reset resources (if needed)
    public void ResetResources()
    {
        resources = 0;
        UpdateResourceUI();
    }
}
