using System.Collections;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }
    private SpriteRenderer spriteRenderer;

    public float swingAngle = 90f; // Total swing angle for melee attacks
    public float swingSpeed = 200f; // Speed of the swing in degrees per second
    public Collider2D hitbox; // Collider for the weapon's hitbox

    private bool isSwinging = false;
    private float swingProgress = 0f; // Tracks swing progress (0 to 1)

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Start a melee attack on left mouse click
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartCoroutine(SwingWeapon());
        }
    }
    public void FlipWeaponSprite(bool isFacingLeft)
    {
        // Flip the weapon sprite vertically when facing left
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.flipY = isFacingLeft;
        }
    }


    private IEnumerator SwingWeapon()
    {
        isSwinging = true;
        hitbox.enabled = true; // Enable the hitbox during the swing

        // Swing forward
        while (swingProgress < 1f)
        {
            float swingStep = swingSpeed * Time.deltaTime / swingAngle;
            swingProgress += swingStep;

            float angleOffset = Mathf.Lerp(-swingAngle / 2, swingAngle / 2, swingProgress);
            transform.localRotation = Quaternion.Euler(0, 0, angleOffset);
            yield return null;
        }

        // Reset swing progress
        swingProgress = 0f;
        transform.localRotation = Quaternion.identity;
        hitbox.enabled = false; // Disable the hitbox after the swing
        isSwinging = false;
    }
}
