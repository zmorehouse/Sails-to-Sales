using UnityEngine;
using TMPro;
using System.Collections; // Required for coroutines 

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText; // Main tutorial text
    public TextMeshProUGUI instructionText; // Smaller instructional text (e.g., "Press E to continue")
    
    private ShipController shipController; // Reference to the ShipController
    private int currentStep = 0;
    private bool isWaitingForWASD = false;

    private string[] tutorialSteps = new string[]
    {
        "Ahoy there, swabbie! Welcome aboard!",
        "So, you've joined the Boogie Merchants, eh? First day, exciting stuff! Hope you packed a lunch.",
        "Right, as a merchant, you'll need to hit your KPIs. Yeah, that's right—KPIs!",
        "What's a KPI, you ask? Key Pirate Indicators... or something like that. Who knows? We just make it up as we go.",
        "Wait—what? You've never worked a real job before? Well, let’s start with the basics, shall we?",
        "Step one: driving the ship. Use those trusty WASD keys to sail the seven seas.",
        "Look at you, a natural! Now, your job is to sail to islands and grab all the resources you can carry.",
        "Not gonna lie, it's a tough gig. There's a quota to hit each day if you want to keep this job. No pressure.",
        "But don’t worry, you’ll get paid in gold for your troubles. Ah, sweet gold!",
        "You can spend that gold on shiny new upgrades in the store. Or don’t—whatever floats your boat. I'm not your boss.",
        "But remember, there’s only so many hours in a day! Once the day’s over, you’ll have to sail back—whether you’ve got goods or not.",
    };

    private bool canProceed = true;

    void Start()
    {
        // Automatically find the ship with the "Player" tag
        GameObject ship = GameObject.FindGameObjectWithTag("Player");
        if (ship != null)
        {
            shipController = ship.GetComponent<ShipController>();
        }

        if (shipController != null)
        {
            shipController.SetMovement(false); // Disable ship movement at the start
        }

        // Display the first tutorial step
        DisplayCurrentStep();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canProceed)
        {
            NextStep();
        }

        if (currentStep == 5 && PlayerStartedDriving() && !isWaitingForWASD)
        {
            StartCoroutine(WASDDelay()); // Start the delay coroutine
        }

        if (currentStep == 11 && ShipReachedIsland())
        {
            canProceed = true;
            NextStep();
        }
    }

    private void DisplayCurrentStep()
    {
        if (currentStep < tutorialSteps.Length)
        {
            tutorialText.text = tutorialSteps[currentStep];
            UpdateInstructionText(); // Update the smaller instruction text

            canProceed = true;
        }

        // Unlock movement when teaching WASD (Step 5)
        if (currentStep == 5 && shipController != null)
        {
            shipController.SetMovement(true); // Enable ship movement
            instructionText.text = "Use WASD to move the ship.";
        }

        // Lock movement again until they reach the island (Step 6)
        if (currentStep == 6 && shipController != null)
        {
            shipController.SetMovement(false); // Disable ship movement again
        }
    }

    private void UpdateInstructionText()
    {
        if (currentStep == 5)
        {
            instructionText.text = "Use WASD to move the ship.";
        }
        else
        {
            instructionText.text = "Press E to continue.";
        }
    }

    private void NextStep()
    {
        if (currentStep < tutorialSteps.Length - 1)
        {
            currentStep++;
            DisplayCurrentStep();
        }
        else
        {
            EndTutorial();
        }
    }

    private bool PlayerStartedDriving()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }

    private bool ShipReachedIsland()
    {
        // Add logic to detect if the player has reached the island
        return false; // Replace with actual condition
    }

    private void EndTutorial()
    {
        tutorialText.text = "Alrighty, let's get you started. Sail over to Tutorial Island and get those resources!"; // Clear tutorial text
        instructionText.text = "Press E on tutorial island"; // Clear the instruction text


            shipController.SetMovement(true); // Enable ship movement after tutorial
    }

    // Coroutine to delay moving to the next tutorial step after WASD movement
    IEnumerator WASDDelay()
    {
        isWaitingForWASD = true; // Prevent multiple calls to this coroutine
        yield return new WaitForSeconds(1.5f); // Wait for 5 seconds while the player sails around
        NextStep(); // Move to the next tutorial step
        isWaitingForWASD = false; // Reset the flag
    }
}
