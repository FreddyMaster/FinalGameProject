using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 1; // Damage dealt to player upon collision

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the projectile hits an enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            // Apply damage to the enemy, assuming it has an EnemyHealth component
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Call TakeDamage on the player
            }

            // Destroy the projectile on impact with an enemy
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
