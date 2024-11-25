using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 25;
    public int currentHealth;

    public HealthBar healthBar;
    public TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthText.text = currentHealth + "/" + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TakeDamage(5);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) {
            Die();
        }

        healthBar.SetHealth(currentHealth);
        healthText.text = currentHealth + "/" + maxHealth;
    }

    void Die()
    {
        Debug.Log("You Died");
    }
}
