using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Reference to the enemy prefab
    public GameObject enemyPrefab;

    // Reference to the health potion prefab
    public GameObject healthPotionPrefab;

    // Reference to the room bounds
    private PolygonCollider2D roomBounds;

    // Reference to the tilemap
    public GameObject doors;

    // Counter to keep track of the number of enemies in the room
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
        // Check if the player has entered a room
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
        // Check if the player is in the room before spawning an enemy
        if (playerInRoom)
        {
            Debug.Log("Player is in room, spawning enemy.");
            // Calculate a random position within the room bounds
            Vector3 randomPosition = new Vector3(
                Random.Range(roomBounds.bounds.min.x, roomBounds.bounds.max.x),
                Random.Range(roomBounds.bounds.min.y, roomBounds.bounds.max.y),
                0);

            // Instantiate the enemy at the random position
            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            Debug.Log("Enemy instantiated at position " + randomPosition);

            // Increment the number of enemies in the room
            numEnemiesInRoom++;
            Debug.Log("Number of enemies in room: " + numEnemiesInRoom);

            // Add a listener to the enemy's death event
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
        // Decrement the number of enemies in the room
        numEnemiesInRoom--;
        Debug.Log("Number of enemies in room: " + numEnemiesInRoom);

        // Disable the tilemap if all enemies in the room are defeated
        if (numEnemiesInRoom == 0)
        {
            // Place a health potion in the center of the room
            Vector3 centerPosition = new Vector3(
                roomBounds.bounds.center.x,
                roomBounds.bounds.center.y,
                0);
            GameObject healthPotion = Instantiate(healthPotionPrefab, centerPosition, Quaternion.identity);
            Debug.Log("Placed health potion at center position " + centerPosition);

            doors.gameObject.SetActive(false);

            // Disable colliders once the room is cleared
            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }
}



