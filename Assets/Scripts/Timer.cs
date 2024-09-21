using UnityEngine;
using TMPro; // Import the TMPro namespace

public class CountdownTimer : MonoBehaviour
{
    public float deliveryTimeLimit = 30.0f;  // Default time limit for each delivery
    private float timeRemaining;             // Remaining time for the current delivery
    public TMP_Text timerText;               // Reference to the TMP_Text component
    private bool isTiming = false;           // Flag to check if the timer is running

    private PlayerCollisionDetection playerCollisionDetection; // Reference to PlayerCollisionDetection script

    void Start()
    {
        // Initialize the timer text (hide it initially)
        timerText.gameObject.SetActive(false);

        // Get the PlayerCollisionDetection script reference
        playerCollisionDetection = FindObjectOfType<PlayerCollisionDetection>();
    }

    void Update()
    {
        // Only update the timer if it's running
        if (isTiming && timeRemaining > 0)
        {
            // Decrease time remaining
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(timeRemaining, 0); // Ensure time doesn’t go below 0

            // Update the timer text
            UpdateTimerText();

            // Check if time has run out
            if (timeRemaining == 0)
            {
                // Handle delivery failure
                if (playerCollisionDetection != null)
                {
                    playerCollisionDetection.DeliveryFailed("The delivery expired! A new package is available at ");
                    playerCollisionDetection.LoseLife();
                }
            }
        }
    }

public void StartDeliveryTimer(float baseTimeLimit)
{
    if (playerCollisionDetection != null)
    {
        // Calculate the total time limit including Pirate Patience upgrade
        float extraTime = playerCollisionDetection.piratePatienceLevel * playerCollisionDetection.extraTimePerTier;
        deliveryTimeLimit = baseTimeLimit + extraTime;
    }
    else
    {
        deliveryTimeLimit = baseTimeLimit;
    }

    timeRemaining = deliveryTimeLimit;
    isTiming = true;

    // Show the timer text
    timerText.gameObject.SetActive(true);
    UpdateTimerText();
}

    public void StopDeliveryTimer()
    {
        // Stop the timer
        isTiming = false;

        // Hide the timer text
        timerText.gameObject.SetActive(false);
    }

    public void ReduceTime(float amount)
    {
        // Reduce the timer by the specified amount
        timeRemaining -= amount;
        timeRemaining = Mathf.Max(timeRemaining, 0); // Ensure time doesn’t go below 0

        // Update the timer text
        UpdateTimerText();

        // Check if time has run out
        if (timeRemaining == 0 && playerCollisionDetection != null)
        {
            playerCollisionDetection.DeliveryFailed("The delivery expired! A new package is available at ");
            playerCollisionDetection.LoseLife();
        }
    }

    void UpdateTimerText()
    {
        // Convert timeRemaining to minutes and seconds
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Format the time string
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
