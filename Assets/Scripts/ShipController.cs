// A script used to control the ship's movement and rotation.

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
    
    void Start()
    {
        ShopLogic = FindObjectOfType<ShopLogic>();
        startingY = transform.position.y;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        if (ShopLogic != null) 
        {
            float speed = ShopLogic.baseSpeed;
            transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self); // Move the ship forward based on the vertical input
        }

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
}
