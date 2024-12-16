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
    public GameObject gameOverMenu;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthText.text = currentHealth + "/" + maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            gameOverMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        healthBar.SetHealth(currentHealth);
        healthText.text = currentHealth + "/" + maxHealth;
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        healthBar.SetHealth(currentHealth);
        healthText.text = currentHealth + "/" + maxHealth;
    }

}
