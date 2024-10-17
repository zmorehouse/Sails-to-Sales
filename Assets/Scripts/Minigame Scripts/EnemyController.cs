using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float followRange = 15.0f;  // Distance within which the enemy locks onto the player
    public float followSpeed = 4.0f;   // Speed at which the enemy follows the player
    public float stopDistance = 1.5f;  // Distance at which the enemy stops near the player

    private GameObject player;         // The player object to follow

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");  // Find the player object by tag
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // If the player is within the follow range, lock on and follow
            if (distanceToPlayer < followRange && distanceToPlayer > stopDistance)
            {
                FollowPlayer();
            }
        }
    }

    // Method to move towards the player
    void FollowPlayer()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        transform.position += directionToPlayer * followSpeed * Time.deltaTime;

        // Optionally rotate to face the player
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }

    // Handle collision with the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Check if the enemy collides with the player
        {
            // The enemy has hit the player, so we destroy the enemy after the hit
            Debug.Log("Enemy hit the player and will despawn.");
            Destroy(gameObject);  // Despawn the enemy after the hit
        }
    }
}
