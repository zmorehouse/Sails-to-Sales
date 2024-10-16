using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawnManager : MonoBehaviour
{

    private int xrange = 25;
    private int zrange = 25; 

    public GameObject[] foodPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.DownArrow)){
        //    SpawnRandomFood();
        //}
    }

    public void SpawnRandomFood(){
        int foodIndex = Random.Range(0, foodPrefabs.Length);
        Vector3 spawnPosition = new Vector3(Random.Range(-xrange, xrange), 0.5f, Random.Range(-zrange, zrange));

        GameObject food = Instantiate(foodPrefabs[foodIndex], spawnPosition, foodPrefabs[foodIndex].transform.rotation);

    }
}
