using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public Camera cam;
    private Rigidbody2D rb; // Rigidbody2D reference for movement
    private Transform character; // Player's transform for rotation
    private Vector2 movement; // Store movement direction
    private Vector2 mousePos;

    private void Start()
    {
        character = GetComponent<Transform>(); // Get the player's transform
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void Update()
    {
        // Capture horizontal and vertical inputs (WASD or arrow keys)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Calculate the movement vector
        movement = new Vector2(horizontalInput, verticalInput).normalized;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // Move the player
        MovePlayer(movement);
    }

    void FixedUpdate()
    {
        // Apply movement based on the current movement vector
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        // Calculate the direction to look at
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // Update camera position
        cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(rb.position.x, rb.position.y, cam.transform.position.z), 0.1f);
    }


    void MovePlayer(Vector2 direction)
    {
        // Normalize the direction vector to ensure consistent movement speed
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }

        // Apply movement
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
    }

}

