using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float patrolRadius = 10f;
    public float fireRate = 0.5f;
    public int shootforce = 3000;
    private Vector3 patrolCenter;
    private NavMeshAgent agent;
    private Transform target;
    private float attackRange = 3f;
    private float shotDelay = 0.5f; 
    private float cooldownTime = 2f; // Cooldown time after an attack sequence
    private float lastShotTime = 0f; // Time of the last shot
    private float lastAttackEndTime = 0f; // Time when the last attack sequence ended
    private int attackCount = 0; // Counter for the number of attacks

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
            // Stop moving and attack if within range
            agent.SetDestination(transform.position); // Stop the agent
            if (Time.time >= lastAttackEndTime + cooldownTime)
            {
                // Check if enough time has passed since the last shot
                if (Time.time >= lastShotTime + 1f / fireRate)
                {
                    // Choose between burst, spread, and spiral attack
                    float randomValue = Random.value;
                    if (randomValue < 0.33f)
                    {
                        StartCoroutine(BurstAttack());
                    }
                    else if (randomValue < 0.66f)
                    {
                        StartCoroutine(SpreadAttack());
                    }
                    else
                    {
                        StartCoroutine(SpiralAttack());
                    }
                    lastShotTime = Time.time; // Update the time of the last shot
                    attackCount++; // Increment the attack counter

                    // Check if 3 attacks have been performed
                    if (attackCount >= 3)
                    {
                        Invoke(nameof(ResetShotTimer), cooldownTime);
                    }
                }
            }
        }
        else
        {
            // Move towards the player if out of range
            agent.SetDestination(target.position);
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

    private IEnumerator BurstAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            Shoot(Vector3.zero); // No spread for burst
            yield return new WaitForSeconds(shotDelay);
        }
    }

    private IEnumerator SpreadAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            Shoot(new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f)));
            yield return new WaitForSeconds(shotDelay);
        }
    }

    private IEnumerator SpiralAttack()
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 spread = new Vector3(
                Mathf.Cos(i * Mathf.PI * 2f / 6f) * 0.5f,
                Mathf.Sin(i * Mathf.PI * 2f / 6f) * 0.5f,
                0);
            Shoot(spread);
            yield return new WaitForSeconds(shotDelay);
        }
    }

    private void Shoot(Vector3 spread)
    {
        // Calculate the direction to the target with reduced accuracy
        Vector3 directionToTarget = (target.position - transform.position).normalized + spread;

        // Instantiate and shoot the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(directionToTarget * shootforce);
        }
        else
        {
            Debug.LogWarning("Projectile is missing Rigidbody component.");
        }
    }

    private void ResetShotTimer()
    {
        lastAttackEndTime = Time.time; // Start cooldown
        attackCount = 0; // Reset attack counter
    }
}
