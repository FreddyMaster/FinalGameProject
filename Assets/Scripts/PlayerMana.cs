using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerMana : MonoBehaviour
{
    public int maxMana = 25;
    public int currentMana;

    public ManaBar manaBar;
    public TextMeshProUGUI manaText;
    public float regenDelay = 0.1f; // Delay between each mana increment

    void Start()
    {
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
        UpdateManaText();
    }

    public void TakeMana(int mana)
    {
        if (currentMana > 0)
        {
            currentMana = Mathf.Max(currentMana - mana, 0); // Prevent negative mana
            manaBar.SetMana(currentMana);
            UpdateManaText();
        }
    }

    public IEnumerator RegenManaCoroutine(int regenRate)
    {
        while (currentMana < maxMana)
        {
            currentMana = Mathf.Min(currentMana + regenRate, maxMana);
            manaBar.SetMana(currentMana);
            UpdateManaText();
            yield return new WaitForSeconds(regenDelay); // Wait before the next increment
        }
    }

    private void UpdateManaText()
    {
        manaText.text = currentMana + "/" + maxMana;
    }
}
