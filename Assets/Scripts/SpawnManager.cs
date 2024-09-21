using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject player; // Reference to the player
    private PlayerCollisionDetection playerCollisionDetection; // Reference to the PlayerCollisionDetection script

    private float spawnRange = 75.0f; // Increased spawn range
    private float minSpawnDistance = 35.0f; // Increased minimum distance from the player
    private float collisionCheckRadius = 20.0f; // Radius to check for nearby colliders

    void Start()
    {
        // Get the reference to the PlayerCollisionDetection script
        playerCollisionDetection = player.GetComponent<PlayerCollisionDetection>();

        if (playerCollisionDetection == null)
        {
            Debug.LogError("PlayerCollisionDetection script not found on the player.");
        }
    }

    void Update()
    {
        // Any necessary update logic
    }

    public void SpawnEnemies()
    {
        int deliveriesCompleted = playerCollisionDetection.GetScore(); // Get the number of deliveries completed from PlayerCollisionDetection
        int enemiesToSpawn = GetNumberOfEnemiesToSpawn(deliveriesCompleted); // Determine how many enemies to spawn based on deliveries

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPos = Vector3.zero; // Initialize spawnPos
            bool validSpawn = false;

            // Ensure valid spawn position
            while (!validSpawn)
{
    // Generate a random angle and radius within the allowed range
    float angle = Random.Range(0f, Mathf.PI * 2);
    float radius = Random.Range(minSpawnDistance, spawnRange); // Ensure it's between min and max distances

    // Convert polar coordinates to cartesian coordinates
    float spawnPosX = player.transform.position.x + radius * Mathf.Cos(angle);
    float spawnPosZ = player.transform.position.z + radius * Mathf.Sin(angle);
    
    spawnPos = new Vector3(spawnPosX, 0, spawnPosZ);

    // Check if the spawn position is far enough from the player
    if (Vector3.Distance(spawnPos, player.transform.position) >= minSpawnDistance && !IsSpawnOnInvalidLocation(spawnPos))
    {
        validSpawn = true;
    }
}

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, enemyPrefab.transform.rotation);

            // Disable enemy movement until player moves
            SimpleEnemy enemyScript = enemy.GetComponent<SimpleEnemy>();
            if (enemyScript != null)
            {
                enemyScript.DisableMovement(); // Ensure movement is disabled initially
            }
        }
    }

    private int GetNumberOfEnemiesToSpawn(int deliveriesCompleted)
    {
        // Gradually ramp up the number of enemies based on deliveries completed
        if (deliveriesCompleted <= 3)
        {
            return 1;
        }
                if (deliveriesCompleted <= 10)
        {
            return Random.Range(1, 3); // 1-2 enemies between 5 and 10 deliveries
        }
 
        else if (deliveriesCompleted < 20)
        {
            return Random.Range(2, 4); // 2-3 enemies between 10 and 20 deliveries
        }
        else if (deliveriesCompleted < 30)
        {
            return Random.Range(3, 5); // 3-4 enemies between 20 and 30 deliveries
        }
        else
        {
            return Random.Range(4, 6); // 4-5 enemies after 30 deliveries
        }
    }

    private bool IsSpawnOnInvalidLocation(Vector3 spawnPos)
    {
        // Check if the spawn position is near any rocks, islands, or shops
        Collider[] colliders = Physics.OverlapSphere(spawnPos, collisionCheckRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Rocks") || collider.CompareTag("Island") || collider.CompareTag("Shop"))
            {
                return true; // Invalid location, there's a nearby rock, island, or shop
            }
        }

        return false; // Valid location, no nearby invalid objects
    }
}
