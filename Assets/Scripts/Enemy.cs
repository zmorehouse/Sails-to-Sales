using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public float speed = 4.0f;       // Speed of the enemy movement
    public float circleRadius = 10.0f; // Radius of the circular path
    public float lockOnRange = 0.2f;  // Range at which the enemy locks onto the player
    private Vector3 circleCenter;    // Center of the circular path
    private float angle = 0.0f;      // Current angle for circular movement
    private GameObject player;       // Reference to the player

    private bool canMove = false; // Flag to control movement
    private Vector3 lastPlayerPosition;
    private float moveThreshold = 3.0f; // Threshold for player movement to enable enemy movement

    void Start()
    {
        // Set a random position for the circle center within a specified range
        circleCenter = new Vector3(Random.Range(-35.0f, 35.0f), 0, Random.Range(-35.0f, 35.0f));
        transform.position = circleCenter;

        // Find the player object in the scene
        player = GameObject.Find("Ship");

        lastPlayerPosition = player.transform.position; // Initialize with player's start position
    }

    void Update()
    {
        // Check if the player has moved significantly to enable enemy movement
        if (!canMove && Vector3.Distance(player.transform.position, lastPlayerPosition) >= moveThreshold)
        {
            EnableMovement(); // Enable movement when the player moves
        }

        // Move towards the player if allowed
        if (canMove)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= lockOnRange)
            {
                // Lock onto the player
                LockOnPlayer();
            }
            else
            {
                // Drive in circles around the spawn point
                DriveInCircles();
            }
        }
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    void DriveInCircles()
    {
        // Increment the angle to move around the circle
        angle += Time.deltaTime * speed;

        // Calculate the new position on the circle
        float x = circleCenter.x + Mathf.Cos(angle) * circleRadius;
        float z = circleCenter.z + Mathf.Sin(angle) * circleRadius;

        // Move the enemy towards the calculated position
        Vector3 targetPosition = new Vector3(x, transform.position.y, z);
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Rotate the enemy towards the direction of movement
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);

        // Move the enemy towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
    }

    void LockOnPlayer()
    {
        // Calculate direction to the player
        Vector3 direction = (player.transform.position - transform.position).normalized;

        // Rotate the enemy towards the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);

        // Move the enemy towards the player
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * 10);
    }
}
