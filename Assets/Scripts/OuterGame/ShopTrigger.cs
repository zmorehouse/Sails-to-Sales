using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopMenuUI;
    private ShopLogic shopLogic;
    private bool isPlayerInRange = false;

    private void Start()
    {
        // Ensure the shop menu is initially hidden
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(false);
        }
        shopLogic = FindObjectOfType<ShopLogic>();
    }

    private void Update()
    {
        // Check if the player is in range and presses the 'E' key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenShopMenu();
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

    private void OpenShopMenu()
    {
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(true);  // Show the shop menu UI
            Time.timeScale = 0f;  // Pause the game
            shopLogic.DisplayMessage("Welcome to the upgrade store!");  // Display the welcome message
        }
    }

    // Make this method public so the button can access it
    public void CloseShopMenu()
    {
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(false);
            Time.timeScale = 1f;  // Resume the game
        }
    }
}
