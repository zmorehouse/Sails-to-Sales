using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopMenuUI; // Reference to the shop menu UI
    private PlayerCollisionDetection playerCollisionDetection;
    private bool isPlayerInRange = false; // Flag to track if the player is within the trigger zone

    private void Start()
    {
        // Ensure the shop menu is initially hidden
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(false);
        }

        // Get the reference to the PlayerCollisionDetection script
        playerCollisionDetection = FindObjectOfType<PlayerCollisionDetection>();
    }

    private void Update()
    {
        // Check if the player is in range and presses the 'E' key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenShopMenu();
            playerCollisionDetection.UpdateBalanceText(); // Update the balance text when entering the shop
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has collided with the sphere
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true; // Set the flag to true when the player enters the trigger zone
            playerCollisionDetection.DisplayMessage("Press 'E' to open the shop."); // Display a message prompting the player to press 'E'
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset the flag when the player leaves the trigger zone
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerCollisionDetection.DisplayMessage(""); // Clear the message
        }
    }

    private void OpenShopMenu()
    {
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(true); // Show the shop menu UI
            Time.timeScale = 0f; // Pause the game while in the shop menu
            playerCollisionDetection.DisplayMessage("Welcome to the upgrade store!"); // Display the welcome message
        }
    }

    public void CloseShopMenu()
    {
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(false); // Hide the shop menu UI
            Time.timeScale = 1f; // Resume the game
        }

        // Reset the message back to the welcome message
        playerCollisionDetection.DisplayMessage("Welcome to the upgrade store!");
    }

    public void PurchaseExtraLife()
    {
        playerCollisionDetection.PurchaseExtraLife(); // Delegate the purchase logic to PlayerCollisionDetection
    }

    public void PurchaseShootingUpgrade()
    {
        playerCollisionDetection.PurchaseShootingUpgrade(); // Delegate the purchase logic to PlayerCollisionDetection
    }
    public void PurchaseSpeedUpgrade1()
    {
        playerCollisionDetection.PurchaseSpeedUpgrade1();
    }

    public void PurchaseSpeedUpgrade2()
    {
        playerCollisionDetection.PurchaseSpeedUpgrade2();
    }

    public void PurchaseSpeedUpgrade3()
    {
        playerCollisionDetection.PurchaseSpeedUpgrade3();
    }
}
