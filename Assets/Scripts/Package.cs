using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    
    public Transform deliveryTarget; // The island where this package should be delivered

    // Optionally, add methods to set or update the delivery target
    public void SetDeliveryTarget(Transform target)
    {
        deliveryTarget = target;
    }
    // Start is called before the first frame update

    public Transform player; // Reference to the player

    private bool isFollowingPlayer = false; // Flag to check if the package is following the player

    void Update()
    {
        if (isFollowingPlayer && player != null)
        {
            // Position the package behind the player
            transform.position = player.position - player.forward * 2.0f; // Adjust offset as needed
            transform.rotation = player.rotation; // Optionally match player's rotation
        }
    }

    public void StartFollowing(Transform playerTransform)
    {
        player = playerTransform;
        isFollowingPlayer = true;
    }

    public void StopFollowing()
    {
        isFollowingPlayer = false;
        player = null;
    }
}