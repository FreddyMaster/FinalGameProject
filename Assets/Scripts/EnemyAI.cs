using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float stoppingDistance = 2f;
    public float patrolRadius = 10f;
    
    public float fireRate = 0.5f;
    public Transform firePoint;
    public int shootforce = 3000;
    private Vector3 patrolCenter;
    private NavMeshAgent agent;
    private Transform target;
    private bool isChasing;
    private float attackRange = 10f;
    private float shotDelay = 0.1f; 
    private float trajectorySpread = 5f;
    private float minDistanceBeforeShooting = 1f;

    private float lastShotTime = 0f; // Time of the last shot
    private float lastAttackEndTime = 0f; // Time of the last attack
    private int attackCount = 0; // Number of attacks performed
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        patrolCenter = transform.position;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        
        if (distance <= attackRange)
        {
            // AI is within attack range, stop chasing
            isChasing = false;

            // Check if enough time has passed since the last shot
            if (Time.time >= lastShotTime + 1f / fireRate)
            {
                // Only shoot if far enough away from the player
                if (distance > minDistanceBeforeShooting)
                {
                    Shoot();
                    lastShotTime = Time.time; // Update the time of the last shot
                }
            }
        }
        else
        {
            // AI is outside attack range, start chasing
            isChasing = true;
        }

        if (isChasing)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        float distance = Vector3.Distance(transform.position, patrolCenter);
        if (distance > patrolRadius)
        {
            agent.SetDestination(patrolCenter);
        }
        else
        {
            Vector3 randomPosition = patrolCenter + new Vector3(
                Random.Range(-patrolRadius, patrolRadius),
                0,
                Random.Range(-patrolRadius, patrolRadius));
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, 1f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    void Shoot()
    {
        // Calculate the direction to the target
        Vector3 directionToTarget = (target.position - firePoint.position).normalized;

        // Apply trajectory spread
        Quaternion rotation = Quaternion.Euler(
            new Vector3(Random.Range(-trajectorySpread, trajectorySpread), Random.Range(-trajectorySpread, trajectorySpread), Random.Range(-trajectorySpread, trajectorySpread)));

        // Instantiate and shoot the projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, rotation * Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Randomly set the projectile to shoot slowly
            float randomSpeedFactor = Random.Range(0.5f, 1f);
            rb.AddForce(directionToTarget * shootforce * randomSpeedFactor);
        }
        else
        {
            Debug.LogWarning("Projectile is missing Rigidbody component.");
        }

        // Introduce delay before next shot
        Invoke(nameof(ResetShotTimer), shotDelay);

        // Increment the attack counter
        attackCount++;

        // Check if 3 attacks have been performed
        if (attackCount >= 3)
        {
            Invoke(nameof(ResetAttackCounter), 1f / fireRate);
        }
    }

    private void ResetShotTimer()
    {
        lastShotTime = Time.time;
    }

    private void ResetAttackCounter()
    {
        attackCount = 0;
    }
}

