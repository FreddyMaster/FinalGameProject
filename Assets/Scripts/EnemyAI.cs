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
    public float accuracyReduction = 0.2f;
    private float shotDelay = 0.1f; 
    private float trajectorySpread = 5f;

    private float lastShotTime = 0f; // Time of the last shot
    

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
        if (distance < stoppingDistance)
        {
            if (distance <= attackRange)
            {
                // Check if enough time has passed since the last shot
                if (Time.time >= lastShotTime + 1f / fireRate)
                {
                    Shoot();
                    lastShotTime = Time.time; // Update the time of the last shot
                }
            }
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
        // Calculate the direction to the target with reduced accuracy
        Vector3 directionToTarget = (target.position - firePoint.position).normalized;
        Vector3 randomOffset = new Vector3(
            Random.Range(-accuracyReduction, accuracyReduction),
            Random.Range(-accuracyReduction, accuracyReduction),
            Random.Range(-accuracyReduction, accuracyReduction));
        Vector3 reducedDirection = directionToTarget + randomOffset.normalized;

        // Apply trajectory spread
        Quaternion rotation = Quaternion.Euler(
            new Vector3(Random.Range(-trajectorySpread, trajectorySpread), Random.Range(-trajectorySpread, trajectorySpread), Random.Range(-trajectorySpread, trajectorySpread)));

        // Instantiate and shoot the projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, rotation * Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Debug.Log("Projectile has a Rigidbody component.");
            Debug.Log(shootforce);
            Debug.Log(directionToTarget);
            rb.AddForce(reducedDirection * shootforce);
        }
        else
        {
            Debug.LogWarning("Projectile is missing Rigidbody component.");
        }

        // Introduce delay before next shot
        Invoke(nameof(ResetShotTimer), shotDelay);
    }

    private void ResetShotTimer()
    {
        lastShotTime = Time.time;
    }
}
