using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Basic Logic inspired by: https://www.youtube.com/watch?v=sPiVz1k-fEs

/**
    Logic for setting an enemy into a "attacking" state and dealing damage to the player
*/
public class EnemyDamage : MonoBehaviour
{
    public Transform target;
    public Transform attackPoint;
    public float attackRange = 0.2f;
    public int damage = 1;
    public float minAttackDistance = 0.5f;
    public float maxAttackDistance = 1f;

    private Animator anim;
    private Rigidbody2D rb;
    private bool isAttacking = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if(isAttacking)
        {
            if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                AttackFinished();
            }
        }

        if(Vector2.Distance(rb.position, target.position) < minAttackDistance && !isAttacking)
        { 
            Attack();
            EventManager.TriggerEvent("attackStart", null);
            isAttacking = true;
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
    }

    public void GetAttack()
    {
        
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach(Collider2D hitObject in hitObjects)
        {
            if(hitObject.name == "hero")
            {
                if(hitObject.TryGetComponent<Health>(out var healthComponent))
                {
                    healthComponent.TakeDamage(damage);
                    Debug.Log(healthComponent.currentHealth);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint  == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void AttackFinished()
    {
        isAttacking = false;
        EventManager.TriggerEvent("attackEnd", null);
    }
}
