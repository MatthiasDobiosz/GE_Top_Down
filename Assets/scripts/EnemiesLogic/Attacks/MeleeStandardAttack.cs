using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
    Normal melee attack: Will check if target is in range of hitbox and execute attack on the spot
*/
public class MeleeStandardAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.2f;
    public int damage = 20;

    public bool TriggerAttackStart(Rigidbody2D rb, Transform target)
    {
        // Check if target is in hitbox
        double distance = Math.Sqrt(Math.Pow(target.position.x - attackPoint.position.x, 2) + Math.Pow(target.position.y - attackPoint.position.y, 2));

        if(distance <= attackRange)
        {
            EventManager.TriggerEvent("attackStart", new Dictionary<string, object> {
                {"body", rb}
            });
            return true;
        }

        return false;
    }

    public void TriggerAttackEnd(Rigidbody2D rb)
    {
        EventManager.TriggerEvent("attackEnd", new Dictionary<string, object> {
            {"body", rb}
        });
    }

    // Damage function that is used as an animation event
    public void ExecuteStandardAttack()
    {
        // Checks for hit
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach(Collider2D hitObject in hitObjects)
        {
            if(hitObject.name == "hero")
            {
                if(hitObject.TryGetComponent<Health>(out var healthComponent))
                {
                    healthComponent.TakeDamage(damage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
