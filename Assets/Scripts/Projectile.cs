using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifespan = 3f; // Time in seconds before the projectile is destroyed
    public int damage = 5; // Damage dealt to player upon collision

    private void Start()
    {
        // Destroy the projectile after a set lifespan to prevent it from existing indefinitely
        Destroy(gameObject, lifespan);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the projectile hits an enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the health component of the enemy
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Call TakeDamage on the player            

            }
        }
        // Destroy the projectile on impact with a player
        Destroy(gameObject);
    }
}

