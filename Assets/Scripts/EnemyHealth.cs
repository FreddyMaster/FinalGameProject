using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public UnityEvent<float> OnHealthChanged;
    public event System.Action OnDeath;
    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged.Invoke(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        OnHealthChanged.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            OnDeath?.Invoke();
            if(gameObject.tag == "Boss")
            {
                SceneManager.LoadScene("WinScreen");
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

