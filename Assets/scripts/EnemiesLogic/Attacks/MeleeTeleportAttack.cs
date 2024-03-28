using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
    Teleportion Melee Attack: Will teleport behind the player when he is in given Range and execute attack.
*/
public class MeleeTeleportAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.05f;
    public float attackDistance = 0.2f;
    public int damage = 20;


    public Vector2 TriggerAttackStart(Rigidbody2D rb, Transform target)
    {
        // Get player facing direction
        float playerDirectionX = target.GetComponent<Player>().GetLastMovementInput().x;
        float playerDirectionY = target.GetComponent<Player>().GetLastMovementInput().y;

        bool facingRight = playerDirectionX >= 0f;
        Vector2 teleportPosition = facingRight ? new(target.position.x - attackDistance, target.position.y ) : new(target.position.x + attackDistance, target.position.y );

        RaycastHit2D hitObject = Physics2D.Linecast(teleportPosition, target.position, 1 << LayerMask.NameToLayer("Obstacles"));

        // if there are no obstacles execute from behind player
        if(hitObject.collider == null)
        {
            rb.position = teleportPosition;
        }
        // Check other side if there are obstacles, if obstacles on both sides --> do the attack on the spot
        else
        {
            Vector2 newTeleportPosition = facingRight ? new(target.position.x + attackDistance, target.position.y ) : new(target.position.x - attackDistance, target.position.y );
            RaycastHit2D newHitObject = Physics2D.Linecast(newTeleportPosition, target.position, 1 << LayerMask.NameToLayer("Obstacles"));

            if(newHitObject.collider == null)
            {
                rb.position = newTeleportPosition;
            }
        }

        EventManager.TriggerEvent("attackStart", new Dictionary<string, object> {
            {"body", rb}
        });

        return new Vector2(playerDirectionX, playerDirectionY);
    }


    public void TriggerAttackEnd(Rigidbody2D rb)
    {
        EventManager.TriggerEvent("attackEnd", new Dictionary<string, object> {
            {"body", rb}
        });
    }

     // Damage function that is used as an animation event
    public void ExecuteTeleportAttack()
    {
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
