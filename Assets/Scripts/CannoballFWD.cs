using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannoballFWD : MonoBehaviour
{
    public float speed = 35.0f;
    public float gravity = -9.8f;  // Simulates the effect of gravity
    private Vector3 velocity;  // The current velocity of the cannonball

    void Start()
    {
        // Set the initial velocity of the cannonball 
        velocity = transform.TransformDirection(Vector3.forward * speed);
    }

    void Update()
    {
        // Apply gravity to the vertical velocity (y-axis)
        velocity.y += gravity * Time.deltaTime;

        // Move the cannonball according to its velocity
        transform.position += velocity * Time.deltaTime;
    }
}
