using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    public float turnSpeed = 90.0f;
    public float horizontalInput;
    public float forwardInput;
    public GameObject cannonballPrefab;
    public GameObject cannonballAltPrefab;
    public GameObject forwardCannonballPrefab;

    private float fireCooldownTimer = 0.0f;
    public float fireCooldown; // Cooldown duration in seconds

    private PlayerCollisionDetection playerCollisionDetection;

    public Image cooldownBar; // Reference to the UI Image for the cooldown bar

    private float startingY; // Variable to store the initial Y position

    void Start()
    {
        // Get reference to the PlayerCollisionDetection script
        playerCollisionDetection = FindObjectOfType<PlayerCollisionDetection>();

        // Assign the fireCooldown based on playerCollisionDetection
        if (playerCollisionDetection != null)
        {
            fireCooldown = playerCollisionDetection.baseCooldown;
        }

        // Ensure the cooldown bar is initially invisible
        if (cooldownBar != null)
        {
            cooldownBar.fillAmount = 0;
            cooldownBar.gameObject.SetActive(false); // Hide the bar initially
        }

        // Store the starting Y position of the ship
        startingY = transform.position.y;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        if (playerCollisionDetection != null)
        {
            float speed = playerCollisionDetection.baseSpeed;
            // Ensure the Y-axis remains constant when moving the ship forward
            transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self);
            // Update the fireCooldown in case it changes due to upgrades
            fireCooldown = playerCollisionDetection.baseCooldown;
        }

        // Apply rotation while keeping the ship on the same Y-axis
        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * horizontalInput);
        float steeringFactor = Mathf.Clamp01(Mathf.Abs(forwardInput));
        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * horizontalInput * steeringFactor);

        // Update the cooldown timer
        if (fireCooldownTimer > 0)
        {
            fireCooldownTimer -= Time.deltaTime;
            UpdateCooldownBar();
        }

        if (Input.GetKeyDown(KeyCode.Space) && fireCooldownTimer <= 0)
        {
            FireCannons();
            fireCooldownTimer = fireCooldown; // Reset the cooldown timer
            cooldownBar.gameObject.SetActive(true); // Show the bar when firing
            UpdateCooldownBar();
        }

        KeepShipWithinBounds();

        // Ensure the Y-axis stays at the starting Y position
        MaintainStartingY();
    }

    void FireCannons()
    {
        Vector3 leftSpawnPosition = transform.position + transform.TransformDirection(Vector3.right * 1.25f);
        Instantiate(cannonballPrefab, leftSpawnPosition, transform.rotation);

        Vector3 rightSpawnPosition = transform.position + transform.TransformDirection(Vector3.left * 1.25f);
        Instantiate(cannonballAltPrefab, rightSpawnPosition, transform.rotation);

        if (playerCollisionDetection != null && playerCollisionDetection.hasForwardShootingUpgrade)
        {
            Vector3 forwardSpawnPosition = transform.position + transform.TransformDirection(Vector3.forward * 2.0f);
            Instantiate(forwardCannonballPrefab, forwardSpawnPosition, transform.rotation);
        }
    }

    void UpdateCooldownBar()
    {
        if (cooldownBar != null)
        {
            float fillAmount = Mathf.Clamp01(fireCooldownTimer / fireCooldown);
            cooldownBar.fillAmount = fillAmount;

            if (fillAmount <= 0)
            {
                cooldownBar.gameObject.SetActive(false);
            }
        }
    }

    void KeepShipWithinBounds()
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

    void MaintainStartingY()
    {
        // Always keep the ship at the starting Y position
        transform.position = new Vector3(transform.position.x, startingY, transform.position.z);
    }
}
