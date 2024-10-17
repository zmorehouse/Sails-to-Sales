using UnityEngine;

public class PersistentUIManager : MonoBehaviour
{
    public static PersistentUIManager instance;  // Singleton instance

    private void Awake()
    {
        // Ensure this object persists across scene changes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Prevent this object from being destroyed
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }
}
