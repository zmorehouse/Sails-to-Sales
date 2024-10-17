using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Needed to move objects between scenes

public class FoodSpawnManager : MonoBehaviour
{
    private int xrange = 25;
    private int zrange = 25; 

    // Assign specific prefabs for trees, coconuts, bananas, and mangoes
    public GameObject treePrefab;
    public GameObject coconutPrefab;
    public GameObject bananaPrefab;
    public GameObject mangoPrefab;

    // Time between spawning new trees
    public float spawnInterval = 30.0f;

    // List to track spawned trees and fruits
    private List<GameObject> spawnedTrees = new List<GameObject>();
    private List<GameObject> spawnedFruits = new List<GameObject>();

    private Scene minigameScene;  // To track the minigame scene

    void Start()
    {
        // Ensure the minigame scene is loaded
        minigameScene = SceneManager.GetSceneByName("minigame");
        if (!minigameScene.IsValid())
        {
            Debug.LogError("Minigame scene not found!");
            return;
        }

        // Spawn 4 trees initially when the minigame starts
        SpawnInitialTrees(4);

        // Start the coroutine to spawn trees over time
        StartCoroutine(SpawnTreesOverTime());
    }

    // Spawns a set number of trees at the start of the game
    void SpawnInitialTrees(int treeCount)
    {
        for (int i = 0; i < treeCount; i++)
        {
            Vector3 treePosition = new Vector3(Random.Range(-xrange, xrange), 0.5f, Random.Range(-zrange, zrange));
            GameObject tree = Instantiate(treePrefab, treePosition, treePrefab.transform.rotation);
            SceneManager.MoveGameObjectToScene(tree, minigameScene);  // Move tree to the minigame scene
            spawnedTrees.Add(tree);  // Track the spawned tree

            // Randomly choose a fruit type for each tree
            string fruitType = ChooseRandomFruit();
            SpawnSpecificFood(fruitType, treePosition);
        }
    }

    // Coroutine to spawn trees at intervals
    IEnumerator SpawnTreesOverTime()
    {
        while (true)
        {
            // Wait for the interval time before spawning the next tree
            yield return new WaitForSeconds(spawnInterval);

            // Spawn a new tree
            Vector3 treePosition = new Vector3(Random.Range(-xrange, xrange), 0.5f, Random.Range(-zrange, zrange));
            GameObject tree = Instantiate(treePrefab, treePosition, treePrefab.transform.rotation);
            SceneManager.MoveGameObjectToScene(tree, minigameScene);  // Move tree to the minigame scene
            spawnedTrees.Add(tree);  // Track the spawned tree

            // Randomly choose a fruit type for the tree
            string fruitType = ChooseRandomFruit();
            SpawnSpecificFood(fruitType, treePosition);
        }
    }

    // Randomly choose a fruit type (coconut, banana, or mango)
    string ChooseRandomFruit()
    {
        string[] fruits = { "coconut", "banana", "mango" };
        int randomIndex = Random.Range(0, fruits.Length);
        return fruits[randomIndex];
    }

    // Spawns a specific food type at a given position near the tree, with 1-4 fruits on the four sides of the tree
    void SpawnSpecificFood(string fruitType, Vector3 treePosition)
    {
        GameObject fruitPrefab = null;

        switch (fruitType)
        {
            case "coconut":
                fruitPrefab = coconutPrefab;
                break;
            case "banana":
                fruitPrefab = bananaPrefab;
                break;
            case "mango":
                fruitPrefab = mangoPrefab;
                break;
        }

        if (fruitPrefab != null)
        {
            // Randomly choose how many fruits to spawn (between 1 and 4)
            int fruitCount = Random.Range(1, 5); // 1 to 4 fruits

            // List of the four possible positions around the tree (top, bottom, left, right)
            List<Vector3> fruitPositions = new List<Vector3>
            {
                new Vector3(treePosition.x + 1, 0.5f, treePosition.z),     // Right
                new Vector3(treePosition.x - 1, 0.5f, treePosition.z),     // Left
                new Vector3(treePosition.x, 0.5f, treePosition.z + 1),     // Top
                new Vector3(treePosition.x, 0.5f, treePosition.z - 1)      // Bottom
            };

            // Shuffle the list to randomize the positions
            for (int i = 0; i < fruitPositions.Count; i++)
            {
                Vector3 temp = fruitPositions[i];
                int randomIndex = Random.Range(i, fruitPositions.Count);
                fruitPositions[i] = fruitPositions[randomIndex];
                fruitPositions[randomIndex] = temp;
            }

            // Spawn fruits in the shuffled positions based on the random fruit count
            for (int i = 0; i < fruitCount; i++)
            {
                GameObject fruit = Instantiate(fruitPrefab, fruitPositions[i], fruitPrefab.transform.rotation);
                SceneManager.MoveGameObjectToScene(fruit, minigameScene);  // Move fruits to the minigame scene
                spawnedFruits.Add(fruit);  // Track the spawned fruit
            }
        }
    }

    // Destroy all spawned objects when leaving the minigame scene
    public void DestroyAllSpawnedObjects()
    {
        // Destroy all spawned trees
        foreach (GameObject tree in spawnedTrees)
        {
            if (tree != null)
            {
                Destroy(tree);
            }
        }

        // Destroy all spawned fruits
        foreach (GameObject fruit in spawnedFruits)
        {
            if (fruit != null)
            {
                Destroy(fruit);
            }
        }

        spawnedTrees.Clear();
        spawnedFruits.Clear();
    }
}
