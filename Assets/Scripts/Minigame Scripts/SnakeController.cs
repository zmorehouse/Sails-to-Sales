using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;  // Required for UI elements
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    public List<GameObject> inventory = new List<GameObject>();
    private List<Vector3> obstacles = new List<Vector3>();
    public List<GameObject> foodObjects = new List<GameObject>();
    public static SnakeController Instance;

    public float movementSpeed = 3.5f;
    public float followDistance = 0.5f;
    private ResourceManager resourceManager;
    private bool inMinigame = true;
    private bool isMovementEnabled = true;

    // Player health variables
    public int maxHealth = 3;
    public int currentHealth;

    // UI Components for Health
    public TextMeshProUGUI healthText;

    // Red flash panel
    public Image redFlashPanel;  // Reference to the red panel

    public bool tutorialActive = false;

    void Start()
    {
        resourceManager = ResourceManager.instance;

        // Initialize player health
        currentHealth = maxHealth;
        UpdateHealthUI();

        // Automatically set tutorialActive based on the scene name
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "MinigameTutorial")
        {
            tutorialActive = true;
            isMovementEnabled = false;
        }
        else
        {
            tutorialActive = false;
            isMovementEnabled = true;
        }
    }

    void Update()
    {
        if (isMovementEnabled)
        {
            moveSnake();

            // Handle left/right rotation input using A and D keys
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.Rotate(0, -90, 0);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.Rotate(0, 90, 0);
            }

            MoveInventory();
        }
    }

    public void SetMovement(bool canMove)
    {
        isMovementEnabled = canMove;
    }

    void moveSnake()
    {
        Vector3 prev = transform.position;
        transform.position += transform.forward * movementSpeed * Time.deltaTime;

        if (!inMap(transform.position) || !noObstacles(transform.position))
        {
            transform.position = prev;
        }
    }

    void MoveInventory()
    {
        Vector3 previousPosition = transform.position;

        for (int i = 0; i < inventory.Count; i++)
        {
            Vector3 targetPosition = previousPosition - transform.forward * followDistance;
            GameObject currentFood = inventory[i];

            currentFood.transform.position = Vector3.Lerp(currentFood.transform.position, targetPosition, Time.deltaTime * movementSpeed);
            previousPosition = currentFood.transform.position;
        }
    }

    bool inMap(Vector3 pos)
    {
        return pos.x < 25 && pos.x > -25 && pos.z < 25 && pos.z > -25;
    }

    bool noObstacles(Vector3 pos)
    {
        foreach (Vector3 obstacle in obstacles)
        {
            if (Mathf.Abs(pos.x - obstacle.x) < 0.5f && Mathf.Abs(pos.z - obstacle.z) < 0.5f)
            {
                return false;
            }
        }
        return true;
    }

    public void PickUpFood(GameObject food)
    {
        inventory.Add(food);
        food.transform.position = transform.position - transform.forward * followDistance;
    }

    // Handle collisions with enemies
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food" && !inventory.Contains(other.gameObject))
        {
            PickUpFood(other.gameObject);

            string foodName = other.gameObject.name.Replace("(Clone)", "").Trim();

            if (foodName.Contains("Coconut"))
            {
                ResourceManager.instance.AddCurrentCoconuts(1);
            }
            else if (foodName.Contains("Mango"))
            {
                ResourceManager.instance.AddCurrentMangoes(1);
            }
            else if (foodName.Contains("Banana"))
            {
                ResourceManager.instance.AddCurrentBananas(1);
            }
        }

        // Logic when hit by an enemy
        if (other.tag == "Enemy" && inMinigame)
        {
            Debug.Log("Player hit by an enemy. Health reduced.");
            TakeDamage(1);
            DropResources();
        }
    }

    // Method to handle taking damage and flashing the screen
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log("Player health is zero. Exiting minigame.");
            DestroyMinigameScene();
        }

        StartCoroutine(FlashRedPanel());  // Flash the red panel when hit
    }

    // Coroutine to flash the red panel when the player is hit
    IEnumerator FlashRedPanel()
    {
        Color panelColor = redFlashPanel.color;

        // Increase alpha to make it visible
        for (float alpha = 0f; alpha <= 0.5f; alpha += Time.deltaTime)
        {
            panelColor.a = alpha;
            redFlashPanel.color = panelColor;
            yield return null;
        }

        // Decrease alpha to hide it again
        for (float alpha = 0.5f; alpha >= 0f; alpha -= Time.deltaTime)
        {
            panelColor.a = alpha;
            redFlashPanel.color = panelColor;
            yield return null;
        }
    }

    // Update the health UI
    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
        }


    }

    private void DropResources()
    {
        resourceManager.ResetCurrentResources();

        foreach (GameObject item in inventory)
        {
            if (item != null)
            {
                item.transform.parent = null;
                Destroy(item);
            }
        }

        inventory.Clear();
        Debug.Log("All following items have been dropped and destroyed.");
    }

    private void DestroyMinigameScene()
    {
        Debug.Log("Destroying minigame scene and exiting.");

        PopupTrigger popupTrigger = FindObjectOfType<PopupTrigger>();
        if (popupTrigger != null)
        {
            popupTrigger.DestroyMinigameScene();
        }
        else
        {
            Debug.LogError("PopupTrigger script not found! Unable to exit minigame.");
        }
    }
}
