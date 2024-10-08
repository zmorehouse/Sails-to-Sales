using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopMenuUI;
    private ShopLogic ShopLogic;
    private ShipController shipController;
    private QuotaManager quotaManager;
    private bool isPlayerInRange = false;
    private IslandTrigger[] islands;

    private void Start()
    {
        // Ensure the shop menu is initially hidden
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(false);
        }
        ShopLogic = FindObjectOfType<ShopLogic>();

        // Find the player (ship) GameObject and get the ShipController component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            shipController = player.GetComponent<ShipController>();
        }

        // Find all the islands in the scene that have the IslandTrigger component
        islands = FindObjectsOfType<IslandTrigger>();

        // Find the QuotaManager in the scene
        quotaManager = FindObjectOfType<QuotaManager>();
    }

    private void Update()
    {
        // Check if the player is in range and presses the 'E' key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            int playerResources = ResourceManager.instance.resources; // Get the player's current resources
            int requiredQuota = quotaManager.currentResourceQuota; // Get the current resource quota

            if (playerResources < requiredQuota)  // Loss condition
            {
                quotaManager.LoseGame();  // Player loses if they have less than the required resources
            }
            else  // Success condition
            {
                IncrementScoreAndMultiplyQuota();  // Increase score and scale the resource quota
                ResourceManager.instance.ResetResources();  // Reset player's resources

                OpenShopMenu();
                ShopLogic.UpdateBalanceText();

                if (shipController != null)
                {
                    shipController.ResetDayLimit();
                    ResetIslands();
                    quotaManager.ResetIslandLimit();
                }
            }
        }
    }

    // Method to handle success: increment score, multiply quota
    private void IncrementScoreAndMultiplyQuota()
    {
        ScoreManager.Instance.IncrementScore();  // Increment the player's score
        quotaManager.currentResourceQuota *= 2;  // Multiply the resource quota for the next cycle
        Debug.Log($"Score incremented. New quota: {quotaManager.currentResourceQuota}");
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

    private void OpenShopMenu()
    {
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(true);  // Show the shop menu UI
            Time.timeScale = 0f;  // Pause the game
            ShopLogic.DisplayMessage("Welcome to the upgrade store!");  // Display the welcome message
        }
    }

    public void CloseShopMenu()
    {
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(false);
            Time.timeScale = 1f;  // Resume the game
        }

        ShopLogic.DisplayMessage("Welcome to the upgrade store!");
    }

    // Reset all islands back to their original state
    private void ResetIslands()
    {
        foreach (IslandTrigger island in islands)
        {
            island.ResetIsland();  // Reset each island
        }
        Debug.Log("All islands have been reset.");
    }
}
