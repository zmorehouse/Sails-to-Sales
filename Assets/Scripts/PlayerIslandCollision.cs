using UnityEngine;

public class PlayerIslandCollision : MonoBehaviour
{
    private PlayerCollisionDetection playerCollisionDetection;

    private void Start()
    {
        // Initialize the reference to the PlayerCollisionDetection script
        playerCollisionDetection = GetComponent<PlayerCollisionDetection>();

        // Check if the reference is correctly assigned
        if (playerCollisionDetection == null)
        {
            Debug.LogError("PlayerCollisionDetection component not found on the player.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Island"))
        {
            Debug.Log("Collision with island detected!");

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero; // Stop movement
            }

            Vector3 pushDirection = transform.position - collision.transform.position;
        }
        else if (collision.gameObject.CompareTag("Shop"))
        {
            Debug.Log("Collision with shop detected!");

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero; // Stop movement
            }

            Vector3 pushDirection = transform.position - collision.transform.position;
        }
        else if (collision.gameObject.CompareTag("Rocks"))
        {
            Debug.Log("Collision with rocks detected!");

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero; // Stop movement
            }

            Vector3 pushDirection = transform.position - collision.transform.position;

            // Lose a life when colliding with rocks
            if (playerCollisionDetection != null)
            {
                  if (playerCollisionDetection.takesDamageFromTerrain)
                {
                    playerCollisionDetection.LoseLife();
                }
                                    Debug.Log("Player is immune to terrain damage. No life lost.");
            }
            else
            {
                Debug.LogError("playerCollisionDetection is not assigned.");
            }
        }
    }
}
