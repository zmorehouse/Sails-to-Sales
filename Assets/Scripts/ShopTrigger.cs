// A script used to manage the player's balance and purchases in the shop.

using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopMenuUI; // Reference to the shop menu UI
    private ShopLogic ShopLogic;
    private bool isPlayerInRange = false; // Flag to track if the player is within the trigger zone

    private void Start()
    {
        // Ensure the shop menu is initially hidden
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(false);
        }
        ShopLogic = FindObjectOfType<ShopLogic>();
    }

    private void Update()
    {
        // Check if the player is in range and presses the 'E' key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenShopMenu();
            ShopLogic.UpdateBalanceText(); 
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
        // Reset the flag when the player leaves the trigger zone
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void OpenShopMenu()
    {
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(true); // Show the shop menu UI
            Time.timeScale = 0f;  // Pause the game
            ShopLogic.DisplayMessage("Welcome to the upgrade store!"); // Display the welcome message
        }
    }

    public void CloseShopMenu()
    {
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(false); 
            Time.timeScale = 1f; // Resume the game
        }

        // Reset the message back to the welcome message
        ShopLogic.DisplayMessage("Welcome to the upgrade store!");
    }

    // Delegate the purchase methods to the ShopLogic script

    public void PurchaseExtraLife()
    {
        ShopLogic.PurchaseExtraLife(); 
    }

    public void PurchaseShootingUpgrade()
    {
        ShopLogic.PurchaseShootingUpgrade(); 
    }
    public void PurchaseSpeedUpgrade1()
    {
        ShopLogic.PurchaseSpeedUpgrade1();
    }

    public void PurchaseSpeedUpgrade2()
    {
        ShopLogic.PurchaseSpeedUpgrade2();
    }

    public void PurchaseSpeedUpgrade3()
    {
        ShopLogic.PurchaseSpeedUpgrade3();
    }
}
