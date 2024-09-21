using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float speed = 40.0f;
    public float gravity = -9.8f;  // Simulates the effect of gravity
    private Vector3 velocity;  // The current velocity of the cannonball

    void Start()
    {
        // Set the initial velocity of the cannonball (moving left relative to the ship)
        velocity = transform.TransformDirection(Vector3.right * speed);
    }

    void Update()
    {
        // Apply gravity to the vertical velocity (y-axis)
        velocity.y += gravity * Time.deltaTime;

        // Move the cannonball according to its velocity
        transform.position += velocity * Time.deltaTime;
    }
}
