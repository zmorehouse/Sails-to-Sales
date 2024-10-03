using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    public float turnSpeed = 90.0f;
    public float horizontalInput;
    public float forwardInput;
    private ShopLogic ShopLogic;
    private float startingY;
    public int dayLimit = 3; // Initial day limit

    void Start()
    {
        startingY = transform.position.y;
        ShopLogic = FindObjectOfType<ShopLogic>(); // Assuming ShopLogic exists in the scene
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        float speed = 10.0f;
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Move the ship forward based on the vertical input

        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * horizontalInput); // Rotate the ship based on the horizontal input
        float steeringFactor = Mathf.Clamp01(Mathf.Abs(forwardInput));
        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * horizontalInput * steeringFactor); 

        KeepShipWithinBounds();
        MaintainStartingY();
    }

    void KeepShipWithinBounds() // Keep the ship within the bounds of the game area
    {
        if (transform.position.x < -50)
        {
            transform.position = new Vector3(-50, startingY, transform.position.z);
        }
        else if (transform.position.x > 50)
        {
            transform.position = new Vector3(50, startingY, transform.position.z);
        }
        else if (transform.position.z < -50)
        {
            transform.position = new Vector3(transform.position.x, startingY, -50);
        }
        else if (transform.position.z > 50)
        {
            transform.position = new Vector3(transform.position.x, startingY, 50);
        }
    }

    void MaintainStartingY()  // Maintain the starting Y position of the ship
    {
        transform.position = new Vector3(transform.position.x, startingY, transform.position.z);
    }

    // Day limit management
    public void DecreaseDayLimit()
    {
        if (dayLimit > 0)
        {
            dayLimit--;
            Debug.Log($"Day limit decreased. Days remaining: {dayLimit}");
        }
        else
        {
            Debug.Log("No more days left to interact with islands.");
        }
    }

    public void ResetDayLimit()
    {
        dayLimit = 3;
        Debug.Log("Day limit reset to 3.");
    }

    public bool CanInteractWithIsland()
    {
        return dayLimit > 0;
    }
}
