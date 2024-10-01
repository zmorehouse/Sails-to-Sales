// A script used to switch between scenes when play is pressed

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void StartGame()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore(); // Reset the score when the game starts
        }

        SceneManager.LoadScene("Game"); 
    }
}
