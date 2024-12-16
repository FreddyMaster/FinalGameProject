using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public PlayerMana playerMana;
    public Transform attackPoint; // The point where the attack originates
    public GameObject pauseMenu;
    public float attackRange = 1f; // Range of the sword's attack
    public LayerMask enemyLayers; // Layer for enemies that can be hit
    public int attackDamage = 10; // Damage dealt by the attack
    public int manaRegenRate = 5;
    private float lastAttackTime = 0f;
    private bool isRegeneratingMana = false;
    private float regenDelay = .5f; // Delay to start regenerating mana after the attack

    void Update()
    {
        // Attack logic
        if (pauseMenu.activeSelf == false && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();

            if (playerMana.currentMana >= 5)
            {
                playerMana.TakeMana(5);
            }
            else
            {
                Debug.Log("Not enough mana to attack!");
            }

            // Reset the timer and stop regeneration if attacking
            lastAttackTime = Time.time;
            if (isRegeneratingMana)
            {
                StopCoroutine(playerMana.RegenManaCoroutine(manaRegenRate));
                isRegeneratingMana = false;
            }
        }

        // Check if enough time has passed since the last attack to start regenerating mana
        if (Time.time >= lastAttackTime + regenDelay && !isRegeneratingMana && playerMana.currentMana < playerMana.maxMana)
        {
            // Start regenerating mana
            StartCoroutine(playerMana.RegenManaCoroutine(manaRegenRate));
            isRegeneratingMana = true;
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
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
        

        // Optional: Add visual or sound effects here
        Debug.Log("Attacked!");
    }

    // Draw the attack range in the Scene view for visualization
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
