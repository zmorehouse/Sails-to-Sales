using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed to manage scene movement

public class SimpleEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;   // Assign your enemy prefab in the Inspector
    public float spawnInterval = 3.0f; // Spawn an enemy every 3 seconds
    public float spawnRange = 25f;   // Range within which enemies can be spawned

    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Track spawned enemies
    private Scene minigameScene; // Reference to the minigame scene

    void Start()
    {
        // Ensure the minigame scene is loaded
        minigameScene = SceneManager.GetSceneByName("minigame");
        if (!minigameScene.IsValid())
        {
            Debug.LogError("Minigame scene not found!");
            return;
        }

        // Start the enemy spawning coroutine
        StartCoroutine(SpawnEnemiesOverTime());
    }

    // Coroutine to spawn enemies over time
    IEnumerator SpawnEnemiesOverTime()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval); // Wait for the interval before spawning the next enemy
        }
    }

    // Method to spawn a new enemy
    void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnRange, spawnRange), 0.5f, Random.Range(-spawnRange, spawnRange));
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Move the enemy to the minigame scene and set as child of the minigame scene
        SceneManager.MoveGameObjectToScene(enemy, minigameScene);
        spawnedEnemies.Add(enemy); // Track the spawned enemy
    }

    // Destroy all spawned enemies when leaving the minigame scene
    public void DestroyAllSpawnedEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        spawnedEnemies.Clear();
    }
}
