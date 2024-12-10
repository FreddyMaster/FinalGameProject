using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    // Patrol points
    public Transform[] patrolPoints;

    // Melee attack range
    public float attackRange = 1f;

    // Melee attack damage
    public int attackDamage = 10;

    // Melee attack cooldown
    public float attackCooldown = 1f;

    // Movement speed
    public float speed = 5f;

    // Current target
    private Transform target;

    // Current patrol point
    private int currentPatrolPoint = 0;

    // NavMeshAgent
    private NavMeshAgent agent;

    // Melee attack timer
    private float attackTimer = 0f;

    void Start()
    {
        // Initialize NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        // Set initial target to player
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Start patrolling
        Patrol();
    }

    void Update()
    {
        // Check if target is in attack range
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget <= attackRange)
        {
            // Attack target
            Attack();
        }
        else
        {
            // Patrol
            Patrol();
        }
    }

    void Patrol()
    {
        // Move to current patrol point
        agent.SetDestination(patrolPoints[currentPatrolPoint].position);

        // Check if arrived at patrol point
        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 0.1f)
        {
            // Move to next patrol point
            currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
        }
    }

    void Attack()
    {
        // Check if attack cooldown is over
        if (attackTimer <= 0f)
        {
            // Deal damage to target
            target.GetComponent<Health>().TakeDamage(attackDamage);

            // Reset attack cooldown
            attackTimer = attackCooldown;
        }
        else
        {
            // Decrease attack cooldown
            attackTimer -= Time.deltaTime;
        }
    }
}

// Simple health script for player
public class Health : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle player death
        Debug.Log("Player died!");
    }
}