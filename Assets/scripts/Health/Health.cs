using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Sets health for a given object and exposes functions to take/heal damage
*/
public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Healthbar healthbar;
    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthbar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
            //Death
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
