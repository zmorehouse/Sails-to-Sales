using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerCollisionDetection : MonoBehaviour
{
    public float timeReward = 15.0f; // Time to add when delivering a package
    public Transform[] islands; // Array of all islands
    public float spawnOffset = 7.5f; // Offset distance from the front of the island

    public int maxLives = 15; // Maximum number of lives
    public int currentLives; // Current lives
    public TextMeshProUGUI lifeText; // Reference to the TextMeshProUGUI component for lives
    public TextMeshProUGUI moneyText; // Reference to the TextMeshProUGUI component for money
    public TextMeshProUGUI packageValueText; // Reference to the TextMeshProUGUI component for package value
        public TextMeshProUGUI messageText; // Reference to the TextMeshProUGUI component for displaying messages
            public TextMeshProUGUI balanceText; // Reference to the TextMeshProUGUI component for displaying balance in the shop

    private bool hasPackage = false; // Track if the player has a package
    
    private Transform currentDeliveryIsland; // The island where the package was delivered
    private Transform currentDeliveryTarget; // The specific target island for the current package
    public GameObject currentPackage; // Reference to the current package
    public PackagePool packagePool; // Reference to the PackagePool
    public SpawnManager spawnManager; // Reference to the SpawnManager
    public TextMeshProUGUI statusText; // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component for score
    public CountdownTimer countdownTimer; // Reference to the CountdownTimer component

    private int score = 0; // Score variable
    public int money = 0; // Money variable
    private int currentPackageValue = 0; // Value of the current package

     public Button speedUpgrade1Button; // Reference to the button for Speed Upgrade 1
    public Button speedUpgrade2Button; // Reference to the button for Speed Upgrade 2
    public Button speedUpgrade3Button; // Reference to the button for Speed Upgrade 3

public int speedUpgradeLevel = 0; // Track the level of speed upgrade purchased (0 to 3)
public float baseSpeed = 10.0f; // Base speed of the ship
public float speedIncrement = 5f; // Speed increment per upgrade

public int piratePatienceLevel = 0; // Tracks the level of Pirate Patience upgrade (0 to 3)
public float extraTimePerTier = 5f; // Extra time added per tier in seconds

    public Button piratePatienceUpgrade1Button;
public Button piratePatienceUpgrade2Button;
public Button piratePatienceUpgrade3Button;

public int cooldownUpgradeLevel = 0; // Track the level of cooldown reduction upgrade purchased (0 to 3)
public float baseCooldown = 1.5f; // Base cooldown duration
public float cooldownReductionPerTier = 20000f; // Cooldown reduction per upgrade tier

public Button cooldownUpgrade1Button; // Reference to the button for Cooldown Upgrade 1
public Button cooldownUpgrade2Button; // Reference to the button for Cooldown Upgrade 2
public Button cooldownUpgrade3Button; // Reference to the button for Cooldown Upgrade 3

public Button forwardShootingUpgradeButton; // Reference to the forward shooting upgrade button

public Button hullUpgradeButton; // Reference to the hull upgrade button
public bool takesDamageFromTerrain = true; // Flag to determine if the ship takes damage from terrain

    private string previousStatusMessage; // Variable to store the previous status message
        private bool isInShopRange = false; // To track if the player is in shop range

    public Image redFlashImage; // Reference to the red flash UI panel
    public float flashDuration = 0.5f; // Duration of the flash effect

    private Coroutine flashCoroutine;


    void Start()
    {

        currentLives = 4; // Initialize lives
        UpdateLifeText(); // Initialize life display
        UpdateMoneyText(); // Initialize money display
        packageValueText.gameObject.SetActive(false); // Hide the package value initially
        UpdateBalanceText();
         UpdateSpeedUpgradeButtons();
         UpdatePiratePatienceUpgradeButtons();
         UpdateCooldownUpgradeButtons();
        // Check the number of islands assigned
        if (islands.Length < 4)
        {
            Debug.LogError("Not enough islands assigned. Ensure exactly 4 islands are assigned to the islands array.");
            return;
        }

        // Get the PackagePool reference
        packagePool = FindObjectOfType<PackagePool>();

        // Get the CountdownTimer reference
        countdownTimer = FindObjectOfType<CountdownTimer>();

        // Spawn an initial package
        SpawnInitialPackage();
        UpdateScoreText(); // Initialize score display
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug the object that collided
        Debug.Log("Collided with: " + other.name);

 if (other.CompareTag("Shop"))
        {
            // Store the current status message before changing it
            previousStatusMessage = statusText.text;

            // Display the "Press E to open the shop" message
            UpdateStatusText($"Press E to open the shop.");
            isInShopRange = true;
        }


        if (other.CompareTag("Package"))
        {
            Debug.Log("Package picked up.");
            // Pick up the package
            hasPackage = true;

            spawnManager.SpawnEnemies();

            // Start the delivery timer with a set time limit
            if (countdownTimer != null)
            {
                countdownTimer.StartDeliveryTimer(15f); // Example time limit, adjust as needed
            }

            // Determine the value of the package
            currentPackageValue = Random.Range(40, 60); // Random amount between $25 and 45
            UpdatePackageValueText(); // Update package value display

            // Show the package value text
            packageValueText.gameObject.SetActive(true);

            // Start the package following the player
            Package packageScript = other.GetComponent<Package>();
            if (packageScript != null)
            {
                packageScript.StartFollowing(transform);
                currentPackage = other.gameObject; // Keep track of the current package
                UpdateStatusText($"Deliver the package to {packageScript.deliveryTarget.name}");
            }
        }
        else if (other.CompareTag("Island"))
        {
            if (hasPackage)
            {
                // Check if the package is being delivered to the correct island
                Package packageScript = currentPackage.GetComponent<Package>();
                if (packageScript != null)
                {
                    if (packageScript.deliveryTarget == other.transform)
                    {
                        Debug.Log("Package delivered correctly.");
                        // Stop the timer when the package is delivered
                        if (countdownTimer != null)
                        {
                            countdownTimer.StopDeliveryTimer();
                        }

                        // Reward time, update score, etc.
                        if (countdownTimer != null)
                        {
                            countdownTimer.ReduceTime(-timeReward); // Add time to the timer
                        }

                        // Reward money for successful delivery
                        money += currentPackageValue; // Add the package value to the player's money
                        UpdateMoneyText(); // Update money display
                        Debug.Log($"Earned ${currentPackageValue} for successful delivery. Total money: ${money}");

                        // Hide the package value text
                        packageValueText.gameObject.SetActive(false);

                        // Record the delivery island and spawn a new package
                        currentDeliveryIsland = other.transform;
                        SpawnNewPackage();
                        UpdateScore(); // Increment the score

                        // Stop following and return the package to the pool
                        packageScript.StopFollowing();
                        if (packagePool != null)
                        {
                            packagePool.ReturnPackage(currentPackage);
                        }
                        currentPackage = null; // Reset the current package

                        UpdateStatusText($"Pick up a package at {currentDeliveryIsland.name}");

                        hasPackage = false; // Reset package status
                    }
                    else
                    {
                        Debug.Log("Incorrect delivery destination. Expected: " + packageScript.deliveryTarget.name + ", but got: " + other.name);
                    }
                }
                else
                {
                    Debug.LogError("No Package script found on the current package.");
                }
            }
        }
        else if (other.CompareTag("EnemyShip"))
        {
            Debug.Log("Hit by enemy ship during delivery.");
            if (hasPackage)
            {
                DeliveryFailed("Your delivery was stolen! A new package is available at ");
            }
            LoseLife();
        }
    }

      void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shop"))
        {
            // When leaving the shop range, reset the status message to its previous state
            UpdateStatusText(previousStatusMessage);
            isInShopRange = false;
        }
    }

public void LoseLife()
{
currentLives = currentLives - 1; // Decrease life count
        UpdateLifeText();

        // Trigger red flash effect
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashRed());

        if (currentLives <= 0)
        {
            // Trigger game over
            Debug.Log("Game Over!");
            HandleGameOver();
        }
    }

      private IEnumerator FlashRed()
    {
        // Set the panel to fully visible red
        redFlashImage.color = new Color(1, 0, 0, 0.5f); // Red with 50% opacity

        // Wait for the duration of the flash
        yield return new WaitForSeconds(flashDuration);

        // Fade the red flash out
        float fadeSpeed = 1f / flashDuration;
        float fadeAmount = 0f;

        while (fadeAmount < 1f)
        {
            fadeAmount += Time.deltaTime * fadeSpeed;
            redFlashImage.color = new Color(1, 0, 0, Mathf.Lerp(0.5f, 0f, fadeAmount)); // Gradually reduce the opacity
            yield return null;
        }

        // Ensure the panel is fully transparent after the fade
        redFlashImage.color = new Color(1, 0, 0, 0f);
    }

private void HandleGameOver()
{

    UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");

}

    public void DeliveryFailed(string failureMessage)
    {
        // Only proceed if the player currently has a package
        if (hasPackage && currentPackage != null)
        {
            // Stop the current delivery
            if (countdownTimer != null)
            {
                countdownTimer.StopDeliveryTimer();
            }

            // Hide the package value text
            packageValueText.gameObject.SetActive(false);

            // Stop the package from following the player
            Package packageScript = currentPackage.GetComponent<Package>();
            if (packageScript != null)
            {
                packageScript.StopFollowing(); // Ensure the package stops following the player
            }

            // Return the package to the pool
            if (packagePool != null)
            {
                packagePool.ReturnPackage(currentPackage);
            }
            currentPackage = null; // Reset the current package

            hasPackage = false; // Reset package status

            // If currentDeliveryIsland is not set, choose a random one
            if (currentDeliveryIsland == null && islands.Length > 0)
            {
                currentDeliveryIsland = islands[Random.Range(0, islands.Length)];
            }

            // Start a new delivery
            SpawnNewPackage();

            // Update the status with the specific failure message
            UpdateStatusText($"{failureMessage}{currentDeliveryIsland.name}");
        }
        else
        {
            // No package is currently being carried, so do nothing
            Debug.Log("Player was hit but no package was being carried. No delivery reset.");
        }
    }

    public void UpdateLifeText()
    {
        if (lifeText != null)
        {
            lifeText.text = $"Lives : {currentLives - 1}";
        }
    }

    public void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = $"Gold : ${money}";
        }
    }

    public void UpdatePackageValueText()
    {
        if (packageValueText != null)
        {
            packageValueText.text = $"+${currentPackageValue}";
        }
    }

    public void SpawnInitialPackage()
    {
        if (packagePool != null)
        {
            // Pick a random island to spawn the initial package
            Transform spawnIsland = islands[Random.Range(0, islands.Length)];

            // Calculate the spawn position with an offset
            Vector3 spawnPosition = spawnIsland.position + (spawnIsland.forward * 7.5f);

            // Get a package from the pool
            currentPackage = packagePool.GetPackage();
            if (currentPackage != null)
            {
                // Set the package position and rotation
                currentPackage.transform.position = spawnPosition;
                currentPackage.transform.rotation = Quaternion.identity;

                // Set the delivery target information for the package
                Package packageScript = currentPackage.GetComponent<Package>();
                if (packageScript != null)
                {
                    currentDeliveryTarget = GetRandomDeliveryTarget(spawnIsland); // Set the delivery target
                    packageScript.SetDeliveryTarget(currentDeliveryTarget); // Assign target to package

                    UpdateStatusText($"Pick up a package at {spawnIsland.name}");

                    // Debug log the target island
                    Debug.Log($"Initial package spawned at {spawnIsland.name}. Deliver to {currentDeliveryTarget.name}.");
                }
                else
                {
                    Debug.LogError("Package prefab does not have a Package script component.");
                }
            }
            else
            {
                Debug.LogError("No package available in the pool.");
            }
        }
        else
        {
            Debug.LogError("PackagePool not found.");
        }
    }

    Transform GetRandomDeliveryTarget(Transform currentIsland)
    {
        // Create a list of possible delivery targets excluding the current island
        List<Transform> possibleTargets = new List<Transform>(islands);
        possibleTargets.Remove(currentIsland);

        // Pick a random target from the remaining islands
        if (possibleTargets.Count > 0)
        {
            return possibleTargets[Random.Range(0, possibleTargets.Count)];
        }

        Debug.LogError("No valid delivery targets found.");
        return null;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Island"))
        {
            // Check if the collision happened with the correct collider
            if (collision.gameObject.GetComponent<SphereCollider>() != null)
            {
                Debug.Log("Collision with island detected!");

                // Prevent movement by stopping or repositioning the player
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 stopDirection = -rb.velocity.normalized;
                    rb.AddForce(stopDirection * 10.0f, ForceMode.Impulse);
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }
    }

   public void SpawnNewPackage()
{
    if (currentDeliveryIsland != null)
    {
        // Create a list of possible spawn islands, excluding the current delivery island
        List<Transform> possibleSpawnIslands = new List<Transform>(islands);
        possibleSpawnIslands.Remove(currentDeliveryIsland);

        // Pick a random island from the remaining islands
        Transform spawnIsland = possibleSpawnIslands[Random.Range(0, possibleSpawnIslands.Count)];

        // Determine the delivery target island, ensuring it's different from the spawn island
        Transform deliveryTarget = GetRandomDeliveryTarget(spawnIsland);

        if (deliveryTarget != null)
        {
            // Calculate the spawn position with an offset
            Vector3 spawnPosition = spawnIsland.position + (spawnIsland.forward * 7.5f);

            // Get a new package from the pool
            GameObject newPackage = packagePool.GetPackage();
            if (newPackage != null)
            {
                // Set the package position and rotation
                newPackage.transform.position = spawnPosition;
                newPackage.transform.rotation = Quaternion.identity;

                // Set the delivery target information on the package
                Package packageScript = newPackage.GetComponent<Package>();
                if (packageScript != null)
                {
                    packageScript.SetDeliveryTarget(deliveryTarget);
                    packageScript.StopFollowing();  // Ensure the package is not following the player

                    // Update current delivery target and island
                    currentDeliveryTarget = deliveryTarget;
                    currentDeliveryIsland = spawnIsland;

                    // Debug the delivery target and spawn island names
                    Debug.Log($"Package spawned at {spawnIsland.name}. Deliver to {deliveryTarget.name}");

                    // Update the status text with the new delivery target
                    UpdateStatusText($"Pick up the package at {spawnIsland.name} and deliver it to {deliveryTarget.name}");
                }
                else
                {
                    Debug.LogError("New package prefab does not have a Package script component.");
                }
            }
            else
            {
                Debug.LogError("No package available in the pool.");
            }
        }
        else
        {
            Debug.LogError("Delivery target for spawn island is not set.");
        }
    }
    else
    {
        Debug.LogError("No valid currentDeliveryIsland found for spawning a new package.");
    }
}


    public void UpdateStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    public void UpdateScore()
    {
        score += 1; // Increment the score
        UpdateScoreText();
        ScoreManager.Instance.IncrementScore();
    }

    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score : {score}";
        }
    }



      public bool hasForwardShootingUpgrade = false; // Flag to check if the upgrade has been purchased

public void PurchaseExtraLife()
    {
        int lifeCost = 100; // Cost of one extra life

        if (money >= lifeCost && currentLives < maxLives)
        {
            // Deduct the money
            money -= lifeCost;
            UpdateMoneyText();
            UpdateBalanceText(); // Update the balance display in the shop

            // Add an extra life
            currentLives += 1;
            UpdateLifeText();

            DisplayMessage("Purchased an extra life!");
        }
        else if (currentLives >= maxLives)
        {
            DisplayMessage("You already have the maximum number of lives!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void PurchaseShootingUpgrade()
    {
        int upgradeCost = 250; // Cost of the shooting upgrade

        if (money >= upgradeCost && !hasForwardShootingUpgrade)
        {
            // Deduct the money
            money -= upgradeCost;
            UpdateMoneyText();
            UpdateBalanceText(); // Update the balance display in the shop

            // Enable the forward shooting upgrade
            hasForwardShootingUpgrade = true;
            forwardShootingUpgradeButton.interactable = false; // Enable the Pirate Patience upgrade 1 button
            DisplayMessage("Purchased forward shooting upgrade!");
        }
        else if (hasForwardShootingUpgrade)
        {
            DisplayMessage("You already have that upgrade!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

 public void UpdateBalanceText()
    {


            balanceText.text = $"Current Balance: ${money}";
    }
      public void DisplayMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
    }

    public void PurchaseSpeedUpgrade1()
    {
        int cost = 75; // Cost of the first speed upgrade

        if (money >= cost && speedUpgradeLevel == 0)
        {
            // Deduct the money and increase speed
            money -= cost;
            baseSpeed += speedIncrement;
            speedUpgradeLevel = 1;
            UpdateMoneyText();
            UpdateBalanceText();
            UpdateSpeedUpgradeButtons();

            DisplayMessage("Purchased Speed Upgrade 1!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void PurchaseSpeedUpgrade2()
    {
        int cost = 150; // Cost of the second speed upgrade

        if (money >= cost && speedUpgradeLevel == 1)
        {
            // Deduct the money and increase speed
            money -= cost;
            baseSpeed += speedIncrement;
            speedUpgradeLevel = 2;
            UpdateMoneyText();
            UpdateBalanceText();
            UpdateSpeedUpgradeButtons();

            DisplayMessage("Purchased Speed Upgrade 2!");
        }
        else if (speedUpgradeLevel < 1)
        {
            DisplayMessage("You need to purchase Speed Upgrade 1 first!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void PurchaseSpeedUpgrade3()
    {
        int cost = 250; // Cost of the third speed upgrade

        if (money >= cost && speedUpgradeLevel == 2)
        {
            // Deduct the money and increase speed
            money -= cost;
            baseSpeed += speedIncrement;
            speedUpgradeLevel = 3;
            UpdateMoneyText();
            UpdateBalanceText();
            UpdateSpeedUpgradeButtons();

            DisplayMessage("Purchased Speed Upgrade 3!");
        }
        else if (speedUpgradeLevel < 2)
        {
            DisplayMessage("You need to purchase Speed Upgrade 2 first!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void UpdateSpeedUpgradeButtons()
    {
        // Enable or disable buttons based on current speed upgrade level
        if (speedUpgrade1Button != null)
        {
            speedUpgrade1Button.interactable = (speedUpgradeLevel == 0);
        }

        if (speedUpgrade2Button != null)
        {
            speedUpgrade2Button.interactable = (speedUpgradeLevel == 1);
        }

        if (speedUpgrade3Button != null)
        {
            speedUpgrade3Button.interactable = (speedUpgradeLevel == 2);
        }
    }

public void PurchasePiratePatienceUpgrade1()
{
    int cost = 75; // Cost of the first Pirate Patience upgrade

    if (money >= cost && piratePatienceLevel == 0)
    {
        // Deduct the money and increase delivery time
        money -= cost;
        piratePatienceLevel = 1;
        UpdateMoneyText();
        UpdateBalanceText();
        UpdatePiratePatienceUpgradeButtons();

        DisplayMessage("Purchased Pirate Patience Upgrade 1!");
    }
    else
    {
        DisplayMessage("You don't have enough gold for that.");
    }
}

public void PurchasePiratePatienceUpgrade2()
{
    int cost = 150; // Cost of the second Pirate Patience upgrade

    if (money >= cost && piratePatienceLevel == 1)
    {
        // Deduct the money and increase delivery time
        money -= cost;
        piratePatienceLevel = 2;
        UpdateMoneyText();
        UpdateBalanceText();
        UpdatePiratePatienceUpgradeButtons();

        DisplayMessage("Purchased Pirate Patience Upgrade 2!");
    }
    else if (piratePatienceLevel < 1)
    {
        DisplayMessage("You need to purchase Pirate Patience Upgrade 1 first!");
    }
    else
    {
        DisplayMessage("You don't have enough gold for that.");
    }
}

public void PurchasePiratePatienceUpgrade3()
{
    int cost = 250; // Cost of the third Pirate Patience upgrade

    if (money >= cost && piratePatienceLevel == 2)
    {
        // Deduct the money and increase delivery time
        money -= cost;
        piratePatienceLevel = 3;
        UpdateMoneyText();
        UpdateBalanceText();
        UpdatePiratePatienceUpgradeButtons();

        DisplayMessage("Purchased Pirate Patience Upgrade 3!");
    }
    else if (piratePatienceLevel < 2)
    {
        DisplayMessage("You need to purchase Pirate Patience Upgrade 2 first!");
    }
    else
    {
        DisplayMessage("You don't have enough gold for that.");
    }
}

public void UpdatePiratePatienceUpgradeButtons()
{
    // Enable or disable buttons based on current Pirate Patience upgrade level
    if (piratePatienceUpgrade1Button != null)
    {
        piratePatienceUpgrade1Button.interactable = (piratePatienceLevel == 0);
    }

    if (piratePatienceUpgrade2Button != null)
    {
        piratePatienceUpgrade2Button.interactable = (piratePatienceLevel == 1);
    }

    if (piratePatienceUpgrade3Button != null)
    {
        piratePatienceUpgrade3Button.interactable = (piratePatienceLevel == 2);
    }
}

public void PurchaseCooldownUpgrade1()
{
    int cost = 75; // Cost of the first cooldown upgrade

    if (money >= cost && cooldownUpgradeLevel == 0)
    {
        // Deduct the money and reduce cooldown
        money -= cost;
        baseCooldown -= cooldownReductionPerTier;
        cooldownUpgradeLevel = 1;
        UpdateMoneyText();
        UpdateBalanceText();
        UpdateCooldownUpgradeButtons();

        DisplayMessage("Purchased Cooldown Reduction Upgrade 1!");
    }
    else
    {
        DisplayMessage("You don't have enough gold for that.");
    }
}

public void PurchaseCooldownUpgrade2()
{
    int cost = 150; // Cost of the second cooldown upgrade

    if (money >= cost && cooldownUpgradeLevel == 1)
    {
        // Deduct the money and reduce cooldown
        money -= cost;
        baseCooldown -= cooldownReductionPerTier;
        cooldownUpgradeLevel = 2;
        UpdateMoneyText();
        UpdateBalanceText();
        UpdateCooldownUpgradeButtons();

        DisplayMessage("Purchased Cooldown Reduction Upgrade 2!");
    }
    else if (cooldownUpgradeLevel < 1)
    {
        DisplayMessage("You need to purchase Cooldown Reduction Upgrade 1 first!");
    }
    else
    {
        DisplayMessage("You don't have enough gold for that.");
    }
}

public void PurchaseCooldownUpgrade3()
{
    int cost = 250; // Cost of the third cooldown upgrade

    if (money >= cost && cooldownUpgradeLevel == 2)
    {
        // Deduct the money and reduce cooldown
        money -= cost;
        baseCooldown = 0;
        cooldownUpgradeLevel = 3;
        UpdateMoneyText();
        UpdateBalanceText();
        UpdateCooldownUpgradeButtons();

        DisplayMessage("Purchased Cooldown Reduction Upgrade 3!");
    }
    else if (cooldownUpgradeLevel < 2)
    {
        DisplayMessage("You need to purchase Cooldown Reduction Upgrade 2 first!");
    }
    else
    {
        DisplayMessage("You don't have enough gold for that.");
    }
}

public void UpdateCooldownUpgradeButtons()
{
    if (cooldownUpgrade1Button != null)
    {
        cooldownUpgrade1Button.interactable = (cooldownUpgradeLevel == 0);
    }

    if (cooldownUpgrade2Button != null)
    {
        cooldownUpgrade2Button.interactable = (cooldownUpgradeLevel == 1);
    }

    if (cooldownUpgrade3Button != null)
    {
        cooldownUpgrade3Button.interactable = (cooldownUpgradeLevel == 2);
    }
}

public void PurchaseHullUpgrade()
{
    int upgradeCost = 250; // Cost of the hull upgrade

    if (money >= upgradeCost && takesDamageFromTerrain)
    {
        // Deduct the money
        money -= upgradeCost;
        UpdateMoneyText();
        UpdateBalanceText(); // Update the balance display in the shop

        // Disable terrain damage
        takesDamageFromTerrain = false;

        // Grey out the button after purchase
        if (hullUpgradeButton != null)
        {
            hullUpgradeButton.interactable = false;
        }

        DisplayMessage("Purchased Hull Upgrade!");
    }
    else if (!takesDamageFromTerrain)
    {
        DisplayMessage("You already have the Hull Upgrade!");
    }
    else
    {
        DisplayMessage("You don't have enough gold for that.");
    }
}

 public int GetScore()
    {
        return score; // Returns the current score (number of deliveries)
    }


}
