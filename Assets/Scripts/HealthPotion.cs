using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
     public int healAmount = 5; // Damage dealt to player upon collision
    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the projectile hits an enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the health component of the enemy
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth?.Heal(healAmount); // Call TakeDamage on the player            
        }
        // Destroy the projectile on impact with a player
        Destroy(gameObject);
    }
}
