using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuotaManager : MonoBehaviour
{
    public int requiredIslandLimit = 3;  // The number of island visits
    public int currentIslandVisits = 0;  // Track current visits

    // Quotas for resources
    public int coconutQuota = 5;
    public int mangoQuota = 5;
    public int bananaQuota = 5;

    public int[] quotaStages = {10, 15, 20, 25, 30}; // Stages for the quotas
    private int quotaStageIndex = 0; // Index to track which stage we're on

    public TextMeshProUGUI timeText; // UI element to display the time (formerly islandLimitText)
    public TextMeshProUGUI quotaText; // UI element to display the quotas
    public TextMeshProUGUI instructionsText; // UI element to display instructions

    // Variables for controlling the lighting
    public Light mainLight;
    public float middayIntensity = 1.0f;
    public float sunsetIntensity = 0.6f;
    public float nightIntensity = 0.2f;
    public Color middayColor = Color.white;
    public Color sunsetColor = new Color(1.0f, 0.5f, 0.3f); // Soft orange for sunset
    public Color nightColor = new Color(0.1f, 0.1f, 0.2f); // Dark blue for night

    private bool hasTurnedInQuota = false;

    private void Start()
    {
        UpdateTimeUI();
        UpdateQuotaUI();
        UpdateInstructions();
        UpdateLighting(); // Initialize lighting at the start
    }

    public void IncrementIslandVisit()
    {
        currentIslandVisits++;
        UpdateTimeUI();   // Update the "time" UI based on visits
        UpdateInstructions();
        UpdateLighting(); // Update lighting when an island is visited
    }

    public bool HasMetIslandLimit()
    {
        return currentIslandVisits >= requiredIslandLimit;
    }

    public void ResetIslandLimit()
    {
        currentIslandVisits = 0;
        hasTurnedInQuota = false;
        Debug.Log("Island visits have been reset for the new cycle.");

        // Progress through the quota stages
        if (quotaStageIndex < quotaStages.Length)
        {
            coconutQuota = quotaStages[quotaStageIndex];
            mangoQuota = quotaStages[quotaStageIndex];
            bananaQuota = quotaStages[quotaStageIndex];
            quotaStageIndex++; // Move to the next quota stage
        }

        UpdateTimeUI();
        UpdateQuotaUI();
        UpdateInstructions();
        UpdateLighting();
    }

    // Update the "time" text based on the number of islands visited
    private void UpdateTimeUI()
    {
        if (timeText != null)
        {
            switch (currentIslandVisits)
            {
                case 0:
                    timeText.text = "Time: 9:00 AM";
                    break;
                case 1:
                    timeText.text = "Time: 12:00 PM";
                    break;
                case 2:
                    timeText.text = "Time: 3:00 PM";
                    break;
                case 3:
                    timeText.text = "Time: 6:00 PM";
                    break;
                default:
                    timeText.text = "Time: 6:00 PM";
                    break;
            }
        }
    }

    // Update quota UI
    private void UpdateQuotaUI()
    {
        if (quotaText != null)
        {
            quotaText.text = $"Quotas - Coconuts: {coconutQuota}, Mangoes: {mangoQuota}, Bananas: {bananaQuota}";
        }
    }

    // Update instructions based on time and island visits
    public void UpdateInstructions()
    {
        if (instructionsText != null)
        {
            switch (currentIslandVisits)
            {
                case 0:
                    instructionsText.text = "The day is ahead of you!";
                    break;
                case 1:
                    instructionsText.text = "Midday! Keep pushing forward.";
                    break;
                case 2:
                    instructionsText.text = "The sun is setting! Almost there...";
                    break;
                case 3:
                    if (!hasTurnedInQuota)
                        instructionsText.text = "The day is over! Turn in your quota... or walk the plank!";
                    break;
                default:
                    if (hasTurnedInQuota)
                        instructionsText.text = "Good job! Here's your reward... now get back to work!";
                    break;
            }
        }
    }

    // Coroutine to reset instructions after a delay
    private IEnumerator ResetInstructionsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        instructionsText.text = "The day is ahead of you!";
    }

    // Update lighting based on the time (island visits)
    private void UpdateLighting()
    {
        if (mainLight != null)
        {
            if (currentIslandVisits == 0 || currentIslandVisits == 1)
            {
                mainLight.intensity = middayIntensity;
                mainLight.color = middayColor;
            }
            else if (currentIslandVisits == 2)
            {
                mainLight.intensity = sunsetIntensity;
                mainLight.color = sunsetColor;
            }
            else if (currentIslandVisits >= requiredIslandLimit && !hasTurnedInQuota)
            {
                mainLight.intensity = nightIntensity;
                mainLight.color = nightColor;
            }
        }
    }

    // Show custom message in the instructions UI
    public void ShowMessage(string message)
    {
        if (instructionsText != null)
        {
            instructionsText.text = message;
        }
    }

    // Call when the player turns in their quota
    public void TurnInQuota()
    {
        hasTurnedInQuota = true;
        UpdateInstructions();
        UpdateLighting(); // Ensure the lighting changes after turning in the quota
    }

    public void LoseGame()
    {
        Debug.Log("Player has failed to meet the quota. Game Over.");
        DestroyDontDestroyOnLoadObjects();
        SceneManager.LoadScene("GameOver");
    }

    private void DestroyDontDestroyOnLoadObjects()
    {
        GameObject ScoreManager = GameObject.Find("ScoreManager");
        if (ScoreManager != null)
        {
            Destroy(ScoreManager);
            Debug.Log("ScoreManager destroyed.");
        }

        GameObject resourceManager = GameObject.Find("ResourceTracker");
        if (resourceManager != null)
        {
            Destroy(resourceManager);
            Debug.Log("ResourceManager destroyed.");
        }

        GameObject UIManager = GameObject.Find("UI");
        if (UIManager != null)
        {
            Destroy(UIManager);
            Debug.Log("UI destroyed.");
        }
    }

    public void ShowAlreadyVisitedMessage()
    {
        ShowMessage("You've already visited that island today!");
    }
}
