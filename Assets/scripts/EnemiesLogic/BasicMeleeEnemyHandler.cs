using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Basic enemy handler: Only exceute one standard melee attack
*/
public class BasicMeleeEnemyHandler : Enemy
{
    public float minDistanceAttack = 0.3f;

    private Animator anim;
    private bool isAttacking = false;
    private MeleeStandardAttack meleeStandardAttack;
    private bool hasAttacked = false;
    private MovePointAroundEntity movePointAroundEntityHandler;


    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        meleeStandardAttack = GetComponent<MeleeStandardAttack>();
        movePointAroundEntityHandler = GetComponent<MovePointAroundEntity>();
    }

    void Update()
    {
        if(isAttacking)
        {
            // When half of animation is over check if player is in hitbox and deal damage
            if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5)
            {
                if(!hasAttacked)
                {
                    meleeStandardAttack.ExecuteStandardAttack();
                    FindObjectOfType<AudioManager>().Play("SawStandardAttack");
                    hasAttacked = true;
                }

            }

            if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                AttackFinished();
            }
        }

 
        if(!isAttacking && hasLineOfSight && !isPlayerDead)
        { 
            Vector2 facingDirection = GetCurrentFacingDirection();
            movePointAroundEntityHandler.MovePoint(facingDirection.x, facingDirection.y, 0);
            bool shouldAttack = meleeStandardAttack.TriggerAttackStart(rb, target);
    
            if(shouldAttack)
            {
                anim.SetTrigger("Attack");
                isAttacking = true;
            }
        }

    }

    public void AttackFinished()
    {
        hasAttacked = false;
        isAttacking = false;
        meleeStandardAttack.TriggerAttackEnd(rb);
    }

    private Vector2 GetCurrentFacingDirection()
    {
        return new Vector2(anim.GetFloat("XInput"), anim.GetFloat("YInput"));
    }
}
