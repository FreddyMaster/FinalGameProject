using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    private Rigidbody2D rb; // Rigidbody2D reference for movement
    private Transform character; // Player's transform for rotation
    private Vector2 movement; // Store movement direction

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

        // Rotate the player to face the direction of movement
        if (movement.magnitude > 0) // Check if there is movement
        {
            RotatePlayer(movement);
        }

        // Move the player
        MovePlayer(movement);
    }

    void FixedUpdate()
    {
        // Apply movement based on the current movement vector
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
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

    void RotatePlayer(Vector2 direction)
    {
        // Calculate the angle to rotate based on the movement direction
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        // Apply the rotation (z-axis for 2D) using Quaternion.Euler
        character.rotation = Quaternion.Euler(0f, 0f, angle);
    }

}
