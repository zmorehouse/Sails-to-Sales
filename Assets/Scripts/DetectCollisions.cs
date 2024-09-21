using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
                // Check if the object that collided has the tag "Cannonball"
        if (other.CompareTag("Cannonball"))
        {
            // Destroy both the enemy and the cannonball
            Destroy(gameObject);
            Destroy(other.gameObject);
        }

        else if (other.CompareTag("Player"))
        {
                        CountdownTimer timer = FindObjectOfType<CountdownTimer>();
            if (timer != null)
            {
            }
            
            Destroy(gameObject);
        }
    }
}
