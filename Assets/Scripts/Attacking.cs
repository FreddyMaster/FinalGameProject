using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public Transform attackPoint; // The point where the attack originates
    public float attackRange = 1f; // Range of the sword's attack
    public LayerMask enemyLayers; // Layer for enemies that can be hit
    public int attackDamage = 10; // Damage dealt by the attack

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    void Attack()
    {
        // Detect enemies in range of the attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage each enemy detected
        foreach (Collider2D enemy in hitEnemies)
        {
            // Call a method on the enemy to take damage
            enemy.GetComponent<EnemyController>().TakeDamage(attackDamage);
        }

        // Optional: Add visual or sound effects here
        Debug.Log("Attacked!");
    }

    // Draw the attack range in the Scene view for visualization
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
