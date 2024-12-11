using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float dashSpeed = 10f; // Dashing speed multiplier
    public float dashDuration = 0.25f; // Duration of the dash
    public float dashCooldown = 1f; // Cooldown time between dashes
    public Camera cam;
    private Rigidbody2D rb; // Rigidbody2D reference for movement
    private Transform character; // Player's transform for rotation
    private Vector2 movement; // Store movement direction
    private Vector2 mousePos;
    private bool isDashing = false;
    private float lastDashTime = 0f;

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

        // Check for dash input and cooldown
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }

        // Move the player
        MovePlayer(movement);
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.MovePosition(rb.position + movement * dashSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Apply movement based on the current movement vector
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }

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

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        StartCoroutine(TempImmunity(1f));
    }

    private IEnumerator TempImmunity(float duration)
    {
        // Implement temporary immunity logic here (e.g., disable taking damage)
        yield return new WaitForSeconds(duration);
        // Re-enable taking damage
    }
}

