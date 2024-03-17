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

    public Healthbar healthbar = null;
    void Start()
    {
        currentHealth = maxHealth;

        if(healthbar)
            healthbar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if(healthbar)
            healthbar.SetHealth(currentHealth);
        
        if(currentHealth <= 0)
        {
            EventManager.TriggerEvent("death", new Dictionary<string, object> {
                {"gameobject", transform.gameObject}
            });             
        } else
        {
            EventManager.TriggerEvent("damageTaken", new Dictionary<string, object> {
                {"gameobject", transform.gameObject}
            });     
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if(healthbar)
            healthbar.SetHealth(currentHealth);
    }
}
