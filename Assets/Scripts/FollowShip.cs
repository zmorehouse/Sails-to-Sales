// A script used to control camera movement

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShip : MonoBehaviour
{
    public GameObject ship;

    void Start()
    {
        // Attempt to find the ship GameObject at the start if it's not assigned in the inspector
        if (ship == null)
        {
            ship = GameObject.FindGameObjectWithTag("Player"); // Assumes the ship has the "Player" tag
        }
    }

    void LateUpdate()
    {
        // Ensure the ship is not null before following it
        if (ship != null)
        {
            transform.position = ship.transform.position + new Vector3(0, 25, 0);
        }
        else
        {
            Debug.LogWarning("Ship GameObject is missing or destroyed.");
        }
    }
}
