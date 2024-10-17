using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandTrigger : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private ShipController shipController;
    private Renderer[] islandRenderers;  // Array to store all renderers of the island
    private bool isUsed = false;
    private Material[] originalMaterials; // Array to store original materials of the island
    public Material greyMaterial; // Assign this in the Inspector to use for greying out the island
    private QuotaManager quotaManager;
    private GameObject[] sceneObjects;  // To hold the original scene objects

    private void Start()
    {
        // Find the player (ship) GameObject and get the ShipController component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            shipController = player.GetComponent<ShipController>();
        }

        // Get all the Renderer components from the island and its children
        islandRenderers = GetComponentsInChildren<Renderer>();

        // Store the original materials of all the renderers
        originalMaterials = new Material[islandRenderers.Length];
        for (int i = 0; i < islandRenderers.Length; i++)
        {
            originalMaterials[i] = islandRenderers[i].material;
        }

        // Find the QuotaManager in the scene
        quotaManager = FindObjectOfType<QuotaManager>();

        // Get all objects in the original scene
        sceneObjects = SceneManager.GetActiveScene().GetRootGameObjects();
    }

    private void Update()
    {
        // Check if the player is in range and presses the 'E' key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (shipController != null && shipController.CanInteractWithIsland())
            {
                if (!isUsed)
                {
                    // Hide the original scene objects by disabling them
                    HideOriginalSceneObjects();

                    SceneManager.LoadScene("minigame", LoadSceneMode.Additive);

                    shipController.DecreaseDayLimit();
                    MakeIslandGrey();
                    isUsed = true;

                    // Tell QuotaManager that the island visit has been incremented
                    quotaManager.IncrementIslandVisit();
                }
                else
                {
                    // Show the message that the island has already been visited
                    quotaManager.ShowAlreadyVisitedMessage();
                }
            }
            else
            {
                Debug.Log("Player has no days left and cannot interact with islands.");
            }
        }
    }

    private void HideOriginalSceneObjects()
    {
        // Check if each object still exists before disabling it
        foreach (GameObject obj in sceneObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);  // Disable each object in the original scene
            }
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

    // Method to make all island objects grey by replacing their materials
    private void MakeIslandGrey()
    {
        if (islandRenderers != null && greyMaterial != null)
        {
            foreach (Renderer renderer in islandRenderers)
            {
                renderer.material = greyMaterial;  // Replace the material with the grey material
            }
            Debug.Log("Island turned grey after interaction.");
        }
    }

    // Method to reset the island to its original material
    public void ResetIsland()
    {
        if (islandRenderers != null)
        {
            for (int i = 0; i < islandRenderers.Length; i++)
            {
                islandRenderers[i].material = originalMaterials[i];  // Restore the original material
            }
        }
        isUsed = false; // Mark the island as not used
        Debug.Log("Island has been reset to its original state.");
    }
}
