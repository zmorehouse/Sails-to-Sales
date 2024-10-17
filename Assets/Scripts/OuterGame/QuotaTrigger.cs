using UnityEngine;

public class QuotaTurnInTrigger : MonoBehaviour
{
    private QuotaManager quotaManager;
    private ResourceManager resourceManager;
    private ShipController shipController;
    private bool isPlayerInRange = false;

    private void Start()
    {
        // Find the QuotaManager in the scene
        quotaManager = FindObjectOfType<QuotaManager>();

        // Find the ResourceManager
        resourceManager = ResourceManager.instance;

        // Find the ShipController
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            shipController = player.GetComponent<ShipController>();
        }
    }

    private void Update()
    {
        // Check if the player is in range and presses the 'E' key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            HandleQuotaTurnIn();
        }
    }

    private void HandleQuotaTurnIn()
    {
        int playerCoconuts = resourceManager.totalCoconuts;  // Track all collected resources, not just current island
        int playerMangoes = resourceManager.totalMangoes;
        int playerBananas = resourceManager.totalBananas;

        int requiredCoconuts = quotaManager.coconutQuota;
        int requiredMangoes = quotaManager.mangoQuota;
        int requiredBananas = quotaManager.bananaQuota;

        // Check if the player has met the quota requirements
        if (playerCoconuts < requiredCoconuts || playerMangoes < requiredMangoes || playerBananas < requiredBananas)
        {
            quotaManager.LoseGame();  // Player did not meet the quota
        }
        else
        {
            // Player met the quota; reward the player
            RewardPlayer();

            quotaManager.TurnInQuota();  // Player successfully turns in the quota
            resourceManager.ResetAllResources();  // Reset only the resources collected on the current island

            if (shipController != null)
            {
                shipController.ResetDayLimit();  // Reset the day limit in the ship controller
                quotaManager.ResetIslandLimit();  // Reset the island visits
            }

            // Reset all islands to be accessible again
            ResetAllIslands();
        }
    }

    private void RewardPlayer()
    {
        // Award 100 gold to the player
        ShopLogic shopLogic = FindObjectOfType<ShopLogic>();  // Find the ShopLogic script
        if (shopLogic != null)
        {
            shopLogic.money += 100;  // Award 100 gold
            shopLogic.UpdateMoneyText();  // Update the UI
            shopLogic.DisplayMessage("You've been awarded 100 gold!");
        }

        // Increment the player's score by 1
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();  // Find the ScoreManager script
        if (scoreManager != null)
        {
            scoreManager.IncrementScore();  // Increment the score by 1
        }
    }

    private void ResetAllIslands()
    {
        // Find all IslandTrigger instances in the scene and reset them
        IslandTrigger[] allIslands = FindObjectsOfType<IslandTrigger>();
        foreach (IslandTrigger island in allIslands)
        {
            island.ResetIsland();  // Reset each island to its original state
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            // Display the "Press E to turn in your quota!" message
            if (quotaManager != null)
            {
                quotaManager.ShowMessage("Press E to turn in your quota!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            // Reset the instructions when the player leaves the range
            if (quotaManager != null)
            {
                quotaManager.UpdateInstructions();
            }
        }
    }
}
