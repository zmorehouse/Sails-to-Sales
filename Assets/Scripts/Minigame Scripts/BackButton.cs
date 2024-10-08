using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandGameManager : MonoBehaviour
{
    // Function to unload the island scene and return to the main game
    public void ReturnToGame()
    {
        SceneManager.UnloadSceneAsync("onislandgame");
        Time.timeScale = 1;
    }

    // Function to increment resource count by 1
    public void IncrementResourceByOne()
    {
        if (ResourceManager.instance != null)
        {
            ResourceManager.instance.AddResources(1);  // Increment resource by 1
        }
        else
        {
            Debug.LogError("ResourceManager instance not found!");
        }
    }
}
