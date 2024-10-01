// This script is responsible for managing the shop UI and player upgrades. It handles the purchase of upgrades and displays messages to the player. 
// It also keeps track of the player's score, money, and lives as these directly correlate with the upgrades available in the shop.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopLogic : MonoBehaviour
{
    public float spawnOffset = 7.5f; 
    // Lives Logic
    public int maxLives = 15; 
    public int currentLives; 

    // Score and Money Logic
    private int score = 0; 
    public int money = 0; 

    // TMP Elements
    public TextMeshProUGUI lifeText; 
    public TextMeshProUGUI moneyText; 
    public TextMeshProUGUI balanceText; 
    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI messageText;

    // UI Elements
    public Button speedUpgrade1Button; 
    public Button speedUpgrade2Button; 
    public Button speedUpgrade3Button; 
    public Button piratePatienceUpgrade1Button;
    public Button piratePatienceUpgrade2Button;
    public Button piratePatienceUpgrade3Button;
    public Button cooldownUpgrade1Button; 
    public Button cooldownUpgrade2Button; 
    public Button cooldownUpgrade3Button; 
    public Button forwardShootingUpgradeButton; 
    public Button hullUpgradeButton; 

    // Track the level of the speed upgrade (0 to 3)
    public int speedUpgradeLevel = 0; 
    public float baseSpeed = 10.0f;
    public float speedIncrement = 5f; 

    // Extra time added per tier in seconds
    public int piratePatienceLevel = 0; 
    public float extraTimePerTier = 5f; 

    // Track the level of the cooldown upgrade (0 to 3)
    public int cooldownUpgradeLevel = 0; 
    public float baseCooldown = 1.5f; 
    public float cooldownReductionPerTier = 20000f; 

    public bool takesDamageFromTerrain = true; 

    public Image redFlashImage; // Reference to the red flash UI panel
    public float flashDuration = 0.5f; // Duration of the flash effect

    private Coroutine flashCoroutine;

    void Start()
    {
        currentLives = 4;
        UpdateLifeText(); 
        UpdateMoneyText(); 
        UpdateBalanceText();
        UpdateSpeedUpgradeButtons();
        UpdatePiratePatienceUpgradeButtons();
        UpdateCooldownUpgradeButtons();
        UpdateScoreText(); 
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.name);

        if (other.CompareTag("EnemyShip"))
        {
            LoseLife();
        }
    }

    public void LoseLife()
    {
        currentLives = currentLives - 1; 
        UpdateLifeText();

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashRed());

        if (currentLives <= 0)
        {
            Debug.Log("Game Over!");
            HandleGameOver();
        }
    }

    private IEnumerator FlashRed()
    {
        redFlashImage.color = new Color(1, 0, 0, 0.5f);

        yield return new WaitForSeconds(flashDuration);
        float fadeSpeed = 1f / flashDuration;
        float fadeAmount = 0f;

        while (fadeAmount < 1f)
        {
            fadeAmount += Time.deltaTime * fadeSpeed;
            redFlashImage.color = new Color(1, 0, 0, Mathf.Lerp(0.5f, 0f, fadeAmount)); 
            yield return null;
        }

        redFlashImage.color = new Color(1, 0, 0, 0f);
    }

    private void HandleGameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
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

    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score : {score}";
        }
    }

    public bool hasForwardShootingUpgrade = false; 

    public void PurchaseExtraLife()
    {
        int lifeCost = 100;

        if (money >= lifeCost && currentLives < maxLives)
        {
            money -= lifeCost;
            UpdateMoneyText();
            UpdateBalanceText();

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
        int upgradeCost = 250; 

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
