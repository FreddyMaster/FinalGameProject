using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // Calculate direction vector from the weapon to the pointer position
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;

        // Rotate the weapon to point towards the pointer position
        transform.right = direction;

        // Flip the sprite when pointing left
        if (direction.x < 0)
        {
            spriteRenderer.flipY = true; // Flip vertically when mouse is to the left
        }
        else
        {
            spriteRenderer.flipY = false; // Reset to normal orientation
        }
    }
}
