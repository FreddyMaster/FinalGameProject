using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject healthPotionPrefab;
    private PolygonCollider2D roomBounds;
    public GameObject doors;
    public Transform pointPosition;
    private int numEnemiesInRoom = 0;
    private bool playerInRoom = false;

    private void Start()
    {
        roomBounds = GetComponent<PolygonCollider2D>();
        Debug.Log("EnemySpawner started.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player entered a room.");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player is in room.");
            playerInRoom = true;
            SpawnEnemy();
            doors.gameObject.SetActive(true);
        }
    }

    private void SpawnEnemy()
    {
        Debug.Log("Spawning enemy in room.");
        if (playerInRoom)
        {
            Debug.Log("Player is in room, spawning enemy.");
            Vector3 randomPosition = new Vector3(
                Random.Range(roomBounds.bounds.min.x, roomBounds.bounds.max.x),
                Random.Range(roomBounds.bounds.min.y, roomBounds.bounds.max.y),
                0);

            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            Debug.Log("Enemy instantiated at position " + randomPosition);

            numEnemiesInRoom++;
            Debug.Log("Number of enemies in room: " + numEnemiesInRoom);

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath += EnemyDeath;
                Debug.Log("Added death event listener to enemy.");
            }
            else
            {
                Debug.LogError("Enemy prefab is missing EnemyHealth component.");
            }
        }
        else
        {
            Debug.Log("Player is not in room, not spawning enemy.");
        }
    }

    private void EnemyDeath()
    {
        Debug.Log("Enemy died.");
        numEnemiesInRoom--;
        Debug.Log("Number of enemies in room: " + numEnemiesInRoom);

        if (numEnemiesInRoom == 0)
        {
            GameObject healthPotion = Instantiate(healthPotionPrefab, pointPosition.transform.position, Quaternion.identity);
            Debug.Log("Placed health potion at point position " + pointPosition);

            doors.gameObject.SetActive(false);
            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }
}

