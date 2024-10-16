using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TutorialIslandTrigger : MonoBehaviour
{
    private bool isPlayerInRange = false;


    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

    }

private void Update()
{
    // Check if the player is in range and presses the 'E' key
    if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
    {

    //  SceneManager.LoadScene("game", LoadSceneMode.Single);
    SceneManager.LoadScene("MinigameTutorial", LoadSceneMode.Single); //Go to minigame tutorial

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

  
}