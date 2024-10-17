    using UnityEngine;
    using TMPro;
    using System.Collections; // Required for coroutines

    public class IslandTutorialManager : MonoBehaviour
    {
        public TextMeshProUGUI tutorialText; // Main tutorial text
        public TextMeshProUGUI instructionText; // Smaller instructional text (e.g., "Press E to continue")

        private SnakeController snakeController; // Reference to the SnakeController
        private int currentStep = 0;
        private bool canProceed = true;

        private string[] tutorialSteps = new string[]
        {
            "Welcome to the island!",
            "Get those legs to work—use A and D to change directions.",
            "Look at you go! See those trees? They're packed with different fruits.",
            "You can pick as many or as few as you like. Or leave with nothing—hey, it’s your call!",
            "Ready to head out? Just head back to your ship and press E to sail away.",
            "One more thing—beware the monkeys! If they catch you, you'll lose everything you've grabbed. So don't get too greedy!"
        };

        private bool isWaitingForMovement = false;

        void Start()
        {
            // Automatically find the snake with the "Player" tag
            GameObject snake = GameObject.FindGameObjectWithTag("Player");
            if (snake != null)
            {
                snakeController = snake.GetComponent<SnakeController>();
            }

            if (snakeController != null)
            {
                snakeController.SetMovement(false); // Disable movement at the start
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
        }

        private void DisplayCurrentStep()
        {
            if (currentStep < tutorialSteps.Length)
            {
                tutorialText.text = tutorialSteps[currentStep];
                UpdateInstructionText(); // Update the smaller instruction text

                canProceed = true;

                if (currentStep == 1) // "Get those legs to work—use A and D to change directions."
                {
                    // Enable movement for 10 seconds and then pause again
                    snakeController.SetMovement(true);
                    StartCoroutine(PauseAfterDelay(5f));
                    instructionText.text = "Use A and D to move.";
                }
                else if (currentStep == 5) // "Beware the monkeys!"
                {
                    instructionText.text = "Press E to continue.";
                }
            }
            else
            {
                EndTutorial();
            }
        }

        private void UpdateInstructionText()
        {
            if (currentStep == 1)
            {
                instructionText.text = "Use A and D to move.";
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

        private void EndTutorial()
        {
            tutorialText.text = "Alright, you're ready! Sail away when you're done."; 
            instructionText.text = "Head back to your ship and press E";
            snakeController.SetMovement(true); // Let the player play freely after the tutorial
        }

        // Coroutine to pause movement after a delay
        private IEnumerator PauseAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            snakeController.SetMovement(false); // Pause movement again after the delay
            canProceed = true;
            NextStep(); // Move to the next tutorial step after pausing
        }
    }
